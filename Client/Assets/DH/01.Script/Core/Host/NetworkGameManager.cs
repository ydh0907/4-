using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TestClient;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace DH
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance;

        [SerializeField] private GameObject Player;

        public Dictionary<ulong, PlayerInfo> players = new();

        public Queue<Action> JopQueue = new Queue<Action>();

        public Action onGameStarted = null;
        public Action onGameEnded = null;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (JopQueue.Count > 0)
            {
                for (int i = 0; i < JopQueue.Count; i++)
                {
                    JopQueue.Dequeue()?.Invoke();
                }
            }

            if (!NetworkManager.IsConnectedClient || !NetworkManager.IsListening)
                LoadSceneManager.Instance.LoadScene(1);
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            base.OnNetworkSpawn();
        }

        public void SyncPlayerList()
        {
            if(!IsHost) return;

            Dictionary<ulong, PlayerInfo> players = new Dictionary<ulong, PlayerInfo>(this.players);

            foreach(var player in players)
            {
                OnValueChangedClientRpc(player.Key, player.Value);
            }

            NetworkServerApprovalManager.Instance.UserLogClientRpc();
        }

        public void PlayerKillCount(ulong id)
        {
            players[id].kill++;
            SetValueServerRpc(id, players[id]);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayerReadyServerRpc(ulong id, bool ready)
        {
            players[id].Ready = ready;
            OnValueChangedClientRpc(id, players[id]);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetValueServerRpc(ulong Key, PlayerInfo Value)
        {
            players[Key] = Value;
            OnValueChangedClientRpc(Key, Value);
        }

        [ClientRpc]
        private void OnValueChangedClientRpc(ulong Key, PlayerInfo Value)
        {
            if (Value == null)
                players.Remove(Key);
            else
                players[Key] = Value;
        }

        public void ServerGameStart()
        {
            if (!IsServer) return;

            Program.Instance.Delete(ConnectManager.Instance.nickname, GetLocalIP());

            NetworkServerApprovalManager.Instance.ApprovalShutdown = true;

            List<Vector3> temp = new List<Vector3>();

            foreach(var player in players)
            {
                Vector3 rand = GM.MapManager.Instance.GetSpawnPosition();

                while(temp.Contains(rand))
                    rand = GM.MapManager.Instance.GetSpawnPosition();

                GameObject p = Instantiate(Player, rand, Quaternion.identity);
                p.GetComponent<NetworkObject>().SpawnAsPlayerObject(player.Key);

                temp.Add(rand);
            }

            GetComponent<NetworkServerTimer>().StartTimer();

            onGameStarted?.Invoke();
        }

        public void ServerGameEnd()
        {
            if (!IsServer) return;

            GameEndClientRpc();
            players.Clear();

            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);

            onGameEnded?.Invoke();
        }

        [ClientRpc]
        public void GameEndClientRpc()
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);

            onGameEnded?.Invoke();
        }

        public static string GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            string localIP = "";

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            Program.Instance.Delete(ConnectManager.Instance.nickname, GetLocalIP());
        }
    }
}
