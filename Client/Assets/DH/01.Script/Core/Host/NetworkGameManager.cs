using AH;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TestClient;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Player = PJH.Player;

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

        public NetworkVariable<bool> IsOnGame = new(false);

        public static bool MatchingServerConnection = false;

        [field: SerializeField] public int maxTime { get; private set; } = 90;
        public NetworkVariable<int> currentTime = new(0);

        private void Awake()
        {
            Instance = this;
            Application.wantsToQuit += CanQuit;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Application.wantsToQuit -= CanQuit;
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

                LoadSceneManager.Instance.LoadScene(2);
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            currentTime.Value = maxTime;
        }

        public void SyncPlayerList()
        {
            if (!IsHost) return;

            Dictionary<ulong, PlayerInfo> players = new Dictionary<ulong, PlayerInfo>(this.users);

            foreach (var player in players)
            {
                OnValueChangedClientRpc(player.Key, player.Value);
            }

            NetworkServerApprovalManager.Instance.UserLogClientRpc();
        }

        public void PlayerKillCount(ulong id)
        {
            users[id].kill++;
            SetValue(id, users[id]);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayerReadyServerRpc(ulong id, bool ready)
        {
            users[id].Ready = ready;
            SetValue(id, users[id]);
            ReadyObjects.Instance.SetNicknameColorClientRpc();
        }

        public void SetValue(ulong Key, PlayerInfo Value)
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

            FindFirstObjectByType<IngameUIToolkit>().SetState();
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
            List<NetworkObjectReference> objects = new List<NetworkObjectReference>();

            foreach (var player in users)
            {
                Vector3 rand = GM.MapManager.Instance.GetSpawnPosition();

                while (temp.Contains(rand))
                    rand = GM.MapManager.Instance.GetSpawnPosition();

                Debug.Log(rand + " : Rand");

                GameObject p = Instantiate(Player, rand, Quaternion.identity);
                p.GetComponent<NetworkObject>().SpawnAsPlayerObject(player.Key);

                NetworkObjectReference reference = p.GetComponent<NetworkObject>();
                CreateCharacterClientRpc(player.Key, reference);

                temp.Add(rand);
                objects.Add(reference);
            }

            OnPlayerSpawnedClientRpc();

            GetComponent<NetworkServerTimer>().StartTimer();

            onGameStarted?.Invoke();

            StartCoroutine(WaitAndSetPosition(temp, objects));

            IsOnGame.Value = true;

            StartCoroutine(StartTimerRoutine());
        }

        private IEnumerator StartTimerRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);

            while (currentTime.Value > 0)
            {
                yield return wait;
                currentTime.Value -= 1;
            }

            GameResultSetting();
        }

        private IEnumerator WaitAndSetPosition(List<Vector3> temp, List<NetworkObjectReference> objects)
        {
            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < objects.Count; i++)
            {
                NetworkObjectReference reference = objects[i];
                Vector3 pos = temp[i];

                SetPositionClientRpc(reference, pos);
            }
        }

        [ClientRpc]
        private void SetPositionClientRpc(NetworkObjectReference reference, Vector3 pos)
        {
            NetworkObject obj = reference;
            if (obj.IsOwner)
            {
                obj.transform.position = pos;
                Debug.Log("i own this");
                Debug.Log(pos);
            }
        }

        [ClientRpc]
        private void OnPlayerSpawnedClientRpc()
        {
            Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            foreach (Player player in players)
            {
                player.StartInit();
            }
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

        public void GameResultSetting()
        {
            if (IsServer)
                Instantiate(Podium).GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

            List<PlayerInfo> list = new();
            foreach (var player in users) list.Add(player.Value);

            PlayerInfo temp;
            for (int j = 0; j < list.Count; j++)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].kill < list[i + 1].kill)
                    {
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
            }

            Transform[] pos = RankingPodium.Instance.GetPositions();

            for (int i = 0; i < list.Count; i++)
            {
                MoveClientOnEndClientRpc(pos[i].position, list[i].ID);
            }

            GameEndClientRpc();
            IsOnGame.Value = false;
        }

        [ClientRpc]
        private void MoveClientOnEndClientRpc(Vector3 pos, ulong id)
        {
            NetworkObject obj = NetworkManager.Singleton.LocalClient.PlayerObject;
            Player player = obj.GetComponent<Player>();
            if (obj.OwnerClientId == id)
            {
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                obj.transform.position = pos;
                player.Model.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                player._lockMovement = true;
                player._lockRotation = true;
            }
        }

        [ClientRpc]
        private void GameEndClientRpc()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void ServerGameEnd()
        {
            NetworkManager.Singleton?.Shutdown();
            Destroy(NetworkManager.Singleton?.gameObject);

            onGameEnded?.Invoke();
            LoadSceneManager.Instance.LoadScene(2);
        }

        public static string GetLocalIP() => Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();

        private bool CanQuit()
        {
            if (!QuitServerHandler.QuitDeletingFlag)
            {
                QuitServerHandler.DeleteImmediately(ConnectManager.Instance.nickname, GetLocalIP());
            }
            return !MatchingServerConnection;
        }
    }
}
