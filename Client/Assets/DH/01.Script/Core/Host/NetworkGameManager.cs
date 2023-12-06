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

            foreach(ulong id in NetworkServerConnectManager.Instance.users.Keys)
            {
                NetworkObject.SpawnAsPlayerObject(id);
            }

            onGameStart?.Invoke();
        }
    }
}
