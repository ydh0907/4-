using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkHostConnectManager : NetworkBehaviour
    {
        public static NetworkHostConnectManager Instance = null;

        public List<ulong> users = new();

        public bool isHandlingConnect = false;

        private object locker = new();

        private void Start()
        {
            Instance = this;

            if (IsServer)
            {
                users.Add(NetworkManager.Singleton.LocalClientId);
                NetworkManager.Singleton.OnClientConnectedCallback += ConnectHandling;
                NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectHandling;
            }
        }

        private void ConnectHandling(ulong id)
        {
            isHandlingConnect = true;

            if (users.Count < 4)
            {
                lock (locker)
                {
                    users.Add(id);
                }
            }
            else
            {
                NetworkManager.Singleton.DisconnectClient(id);
            }

            isHandlingConnect = false;
        }

        private void DisconnectHandling(ulong id)
        {
            isHandlingConnect = true;

            users.Remove(id);

            isHandlingConnect = false;
        }
    }
}
