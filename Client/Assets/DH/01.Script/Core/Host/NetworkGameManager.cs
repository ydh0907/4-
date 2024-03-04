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
using Unity.Services.Matchmaker.Models;
using GM;
using HB;

namespace DH
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance;

        [SerializeField] GameObject Player;
        [SerializeField] GameObject Podium;

        [SerializeField] public List<GameObject> Characters = new();
        [SerializeField] public List<GameObject> Drinks = new();

        public Dictionary<ulong, PlayerInfo> users = new();

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
                users.Clear();

                NetworkManager.Singleton.Shutdown();
                Destroy(NetworkManager.Singleton.gameObject);

                LoadSceneManager.Instance.LoadScene(1);
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        public void SyncPlayerList()
        {
            if(!IsHost) return;

            Dictionary<ulong, PlayerInfo> players = new Dictionary<ulong, PlayerInfo>(this.users);

            foreach(var player in players)
            {
                OnValueChangedClientRpc(player.Key, player.Value);
            }

            NetworkServerApprovalManager.Instance.UserLogClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayerKillCountServerRpc(ulong id)
        {
            users[id].kill++;
            SetValueServerRpc(id, users[id]);
            FindFirstObjectByType<IngameUIToolkit>().SetStateClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayerReadyServerRpc(ulong id, bool ready)
        {
            users[id].Ready = ready;
            SetValueServerRpc(id, users[id]);
            ReadyObjects.Instance.SetNicknameColorClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetValueServerRpc(ulong Key, PlayerInfo Value)
        {
            users[Key] = Value;
            OnValueChangedClientRpc(Key, Value);
        }

        [ClientRpc]
        private void OnValueChangedClientRpc(ulong Key, PlayerInfo Value)
        {
            if (Value == null)
                users.Remove(Key);
            else
                users[Key] = Value;
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

            foreach(var player in users)
            {
                Vector3 rand = GM.MapManager.Instance.GetSpawnPosition();

                while(temp.Contains(rand))
                    rand = GM.MapManager.Instance.GetSpawnPosition();

                GameObject p = Instantiate(Player, rand, Quaternion.identity);
                p.GetComponent<NetworkObject>().SpawnAsPlayerObject(player.Value.ID);

                NetworkObjectReference reference = p.GetComponent<NetworkObject>();
                CreateCharacterClientRpc(player.Key, reference);

                temp.Add(rand);
            }

            GetComponent<NetworkServerTimer>().StartTimer();

            SpawnMentos.Instance.StartSpawn();

            onGameStarted?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        public void UnspawnMentosServerRpc()
        {
            SetUnspawnMentosClientRpc();
        }

        [ClientRpc]
        private void SetUnspawnMentosClientRpc()
        {
            SpawnMentos.Instance.StopSpawn();
        }

        [ClientRpc]
        private void CreateCharacterClientRpc(ulong id, NetworkObjectReference reference)
        {
            PlayerInfo player = users[id];
            NetworkObject networkObject = reference;
            GameObject p = networkObject.gameObject;

            Instantiate(Characters[(int)player.Char], p.transform);
            Instantiate(Drinks[(int)player.Cola], p.transform);

            p.transform.Find("Nickname").GetComponent<TextMeshPro>().text = player.Nickname;
        }

        [ClientRpc]
        public void GameResultSettingClientRpc()
        {
            if(IsServer)
                Instantiate(Podium).GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

            List<PlayerInfo> list = new();
            foreach (var player in users) list.Add(player.Value);

            PlayerInfo temp;
            for(int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].kill < list[i + 1].kill)
                {
                    temp = list[i];
                    list[i] = list[i + 1];
                    list[i + 1] = temp;
                }
            }

            Transform[] pos = RankingPodium.Instance.GetPositions();

            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == NetworkManager.LocalClientId)
                {
                    NetworkObject obj = NetworkManager.ConnectedClients[list[i].ID].PlayerObject;
                    obj.transform.position = pos[i].position;
                    obj.transform.rotation = pos[i].rotation;
                    obj.GetComponent<PlayerMovement>().enabled = false;
                    obj.GetComponent<PlayerDamageble>().enabled = false;
                    obj.GetComponent<PlayerKnockback>().enabled = false;
                    obj.GetComponent<PlayerAttack>().enabled = false;
                    obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
                }
            }
        }

        [ClientRpc]
        public void DeleteTransformClientRpc()
        {
            NetworkManager.ConnectedClients[NetworkManager.LocalClientId].PlayerObject.GetComponent<ClientNetworkTransform>().enabled = false;
        }

        public void ServerGameEnd()
        {
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
