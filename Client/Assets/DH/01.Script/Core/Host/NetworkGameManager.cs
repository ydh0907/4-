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
using AH;
using Unity.VisualScripting;
using TMPro;

namespace DH
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance;

        [SerializeField] GameObject Player;

        [SerializeField] public List<GameObject> Characters = new();
        [SerializeField] public List<GameObject> Drinks = new();

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
            {
                players.Clear();

                NetworkManager.Singleton.Shutdown();
                Destroy(NetworkManager.Singleton.gameObject);

                LoadSceneManager.Instance.LoadScene(1);
            }
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
            SetValueServerRpc(id, players[id]);
            ReadyObjects.Instance.SetNicknameColorClientRpc();
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

        [ServerRpc]
        public void UILoadServerRpc()
        {
            UILoadClientRpc();
        }

        [ClientRpc]
        private void UILoadClientRpc()
        {
            if (IsHost) return;

            IngameUIToolkit ingame = GameObject.Find("UIDocument").GetComponent<IngameUIToolkit>();
            ingame._container.Clear();
            ingame.Counter();
        }

        public void ServerGameStart()
        {
            if (!IsServer) return;

            Program.Instance.Delete(ConnectManager.Instance.nickname, GetLocalIP());

            NetworkServerApprovalManager.Instance.ApprovalShutdown = true;

            ReadyObjects.Instance.RemoveClientRpc();

            List<Vector3> temp = new List<Vector3>();

            foreach(var player in players)
            {
                Vector3 rand = GM.MapManager.Instance.GetSpawnPosition();

                while(temp.Contains(rand))
                    rand = GM.MapManager.Instance.GetSpawnPosition();

                GameObject p = Instantiate(Player, rand, Quaternion.identity);
                p.GetComponent<NetworkObject>().SpawnAsPlayerObject(player.Key);

                Instantiate(Characters[(int)player.Value.Char], p.transform);
                Instantiate(Drinks[(int)player.Value.Cola], p.transform);

                p.transform.Find("Nickname").GetComponent<TextMeshPro>().text = player.Value.Nickname;

                temp.Add(rand);
            }

            GetComponent<NetworkServerTimer>().StartTimer();

            onGameStarted?.Invoke();
        }

        public void ServerGameEnd()
        {
            if (!IsServer) return;

            NetworkManager.Singleton?.Shutdown();
            Destroy(NetworkManager.Singleton?.gameObject);

            onGameEnded?.Invoke();
            LoadSceneManager.Instance.LoadScene(1);
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
