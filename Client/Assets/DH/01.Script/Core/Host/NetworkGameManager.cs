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

            GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(GameStart);
            GameObject.Find("EndButton").GetComponent<Button>().onClick.AddListener(GameEnd);
        }

        public void GameStart()
        {
            if (!IsServer) return;

            foreach(var player in players)
            {
                Instantiate(Player).GetComponent<NetworkObject>().SpawnAsPlayerObject(player.ID);
            }

            GetComponent<NetworkServerTimer>().StartTimer();

            onGameStarted?.Invoke();
        }

        public void GameEnd()
        {
            if (!IsServer) return;

            NetworkManager.Singleton.Shutdown();

            onGameEnded?.Invoke();
        }
    }
}
