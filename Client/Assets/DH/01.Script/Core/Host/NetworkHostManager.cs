using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DH
{
    public class NetworkHostManager : NetworkBehaviour
    {
        public static NetworkHostManager Instance = null;

        public UnityEvent onGameStart = new();
        public UnityEvent onGameEnded = new();

        private void Awake()
        {
            Instance = this;
        }

        [ServerRpc]
        public void StartGameServerRpc()
        {
            if(IsHost)
                StartCoroutine(Starting());
        }

        private IEnumerator Starting()
        {
            while (NetworkHostConnectManager.Instance.isHandlingConnect) yield return null;

            //NetworkObject host
            foreach(ulong id in NetworkHostConnectManager.Instance.users)
            {
                //NetworkManager.Singleton.GetComponent<>
            }

            onGameStart?.Invoke();
        }
    }
}
