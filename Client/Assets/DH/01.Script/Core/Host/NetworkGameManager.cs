using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DH
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance = null;

        [SerializeField] private GameObject Player;
        public UnityEvent onGameStart = new();
        public UnityEvent onGameEnded = new();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Instance = this;

            GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
            if (IsHost) StartGameServerRpc();
        }

        [ServerRpc]
        private void StartGameServerRpc()
        {
            StartCoroutine(Starting());
        }

        private IEnumerator Starting()
        {
            while (NetworkServerConnectManager.Instance.isHandlingConnect) yield return null;

            foreach(var info in NetworkServerConnectManager.Instance.users)
            {
                GameObject player = Instantiate(Player, Vector3.zero, Quaternion.identity);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(info.Key);
                player.name = info.Value;
            }

            onGameStart?.Invoke();
        }
    }
}
