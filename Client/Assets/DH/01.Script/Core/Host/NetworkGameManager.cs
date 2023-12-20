using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DH
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance;

        [SerializeField] private GameObject Player;
        [SerializeField] private List<Transform> Spawns = new List<Transform>();

        public List<PlayerInfo> players = new();

        public Action onGameStarted = null;
        public Action onGameEnded = null;

        private void Awake()
        {
            Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            base.OnNetworkSpawn();

            GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(ServerGameStart);
            GameObject.Find("EndButton").GetComponent<Button>().onClick.AddListener(ServerGameEnd);
        }

        public void ServerGameStart()
        {
            if (!IsServer) return;

            List<int> temp = new List<int>();

            foreach(var player in players)
            {
                int rand = UnityEngine.Random.Range(0, Spawns.Count);

                while(temp.Contains(rand))
                    rand = UnityEngine.Random.Range(0, Spawns.Count);

                GameObject p = Instantiate(Player, Spawns[rand].position, Quaternion.identity);
                p.GetComponent<NetworkObject>().SpawnAsPlayerObject(player.ID);

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

            LoadSceneManager.Instance.LoadScene(1);
        }

        [ClientRpc]
        public void GameEndClientRpc()
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);

            onGameEnded?.Invoke();

            LoadSceneManager.Instance.LoadScene(1);
        }
    }
}
