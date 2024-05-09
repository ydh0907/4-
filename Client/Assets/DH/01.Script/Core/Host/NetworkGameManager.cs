using AH;
using Cysharp.Threading.Tasks;
using GM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TestClient;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Player = PJH.Player;
using Random = UnityEngine.Random;

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
        public Dictionary<ulong, NetworkObject> p_NetObj = new();

        public Queue<Action> JopQueue = new Queue<Action>();

        public Action onGameStarted = null;
        public Action onGameEnded = null;

        public NetworkVariable<bool> IsOnGame = new(false);
        public NetworkVariable<bool> IsGameEnd = new(false);

        public static bool MatchingServerConnection = false;

        [field: SerializeField] public int maxTime { get; private set; } = 90;
        [SerializeField] private AudioClip _ingameBGM;
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
                NetworkManager.Shutdown();
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

        public void MakePlayerInstance(ulong id)
        {
            GameObject p = Instantiate(Player);
            NetworkObject obj = p.GetComponent<NetworkObject>();
            p.SetActive(false);

            p_NetObj.Add(id, obj);
        }

        public void ServerGameStart()
        {
            if (!IsServer) return;

            NetworkServerApprovalManager.Instance.ApprovalShutdown = true;

            List<Vector3> pos = MapManager.Instance.GetSpawnList();
            List<NetworkObjectReference> objects = new List<NetworkObjectReference>();

            foreach (var player in users)
            {
                p_NetObj[player.Key].gameObject.SetActive(true);
                int index = Random.Range(0, pos.Count);
                p_NetObj[player.Key].transform.position = pos[index];
                pos.RemoveAt(index);
                p_NetObj[player.Key].SpawnAsPlayerObject(player.Key);
                NetworkObjectReference reference = p_NetObj[player.Key];
                CreateCharacterClientRpc(player.Key, reference);

                objects.Add(reference);
            }

            OnPlayerSpawnedClientRpc();

            StartCoroutine(StartTimerRoutine());

            IsOnGame.Value = true;
            onGameStarted?.Invoke();
            Program.Instance.Delete(ConnectManager.Instance.nickname, GetJoinCode());
        }

        private IEnumerator StartTimerRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);

            while (currentTime.Value > 0)
            {
                yield return wait;
                currentTime.Value -= 1;
            }

            SoundManager.Instance.Clear();
            GameResultSetting();
        }

        //private IEnumerator WaitAndSetPosition(List<Vector3> temp, List<NetworkObjectReference> objects)
        //{
        //    yield return new WaitForSeconds(0.2f);

        //    for (int i = 0; i < objects.Count; i++)
        //    {
        //        NetworkObjectReference reference = objects[i];
        //        Vector3 pos = temp[i];

        //        SetPositionClientRpc(reference, pos);
        //    }
        //}

        //[ClientRpc]
        //private void SetPositionClientRpc(NetworkObjectReference reference, Vector3 pos)
        //{
        //    NetworkObject obj = reference;
        //    if (obj.IsOwner)
        //    {
        //        obj.transform.position = pos;
        //    }
        //}

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

            ReadyObjects.Instance.Remove();
            SoundManager.Instance.Play("BGM/IngameBGM", Sound.Bgm);
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
            IsOnGame.Value = false;
            IsGameEnd.Value = true;
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

            foreach (Unity.Netcode.NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var p = client.PlayerObject.GetComponent<Player>();
                p.Respawn();
            }

            for (int i = 0; i < list.Count; i++)
            {
                NetworkObject obj = NetworkManager.Singleton.ConnectedClients[list[i].ID].PlayerObject;
                NetworkObjectReference NOF = obj;
                MoveClientOnEndClientRpc(pos[i].position, NOF);
            }

            GameEndClientRpc();
        }

        [ClientRpc]
        private void MoveClientOnEndClientRpc(Vector3 pos, NetworkObjectReference nof)
        {
            NetworkObject obj = nof;
            Player player = obj.GetComponent<Player>();
            if (obj.OwnerClientId == NetworkManager.LocalClientId)
            {
                obj.GetComponent<Rigidbody>().isKinematic = true;
                obj.transform.position = pos;
                player.Model.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            player.StopMove = true;
            player._lockMovement = true;
            player._lockRotation = true;
            player.Animator.SetFloat("InputMagnitude", 0);
            player.Animator.SetBool("IsGrounded", true);
            player.Animator.Play("Free Locomotion");
            player.HealthBar.SetActive(false);
            player.enabled = false;
        }

        [ClientRpc]
        private void GameEndClientRpc()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void ServerGameEnd()
        {
            onGameEnded?.Invoke();
            NetworkManager.Shutdown();
            LoadSceneManager.Instance.LoadScene(2);
        }

        public static string GetJoinCode() => NetworkHost.Instance.JoinCode;

        private bool CanQuit()
        {
            if (!QuitServerHandler.QuitDeletingFlag)
            {
                QuitServerHandler.DeleteImmediately(ConnectManager.Instance.nickname, GetJoinCode());
            }

            return !MatchingServerConnection;
        }
    }
}