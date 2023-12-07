using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkServerConnectManager : NetworkBehaviour
    {
        public static NetworkServerConnectManager Instance = null;

        public Dictionary<ulong, string> users = new();

        public bool isHandlingConnect = false;

        private void Start()
        {
            Instance = this;

            if (IsHost)
            {
                users.Add(NetworkManager.Singleton.LocalClientId, GameManager.Instance.nickname);
            }

            if (IsServer)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback += ConnectApproval;
                NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectHandling;
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback -= ConnectApproval;
                NetworkManager.Singleton.OnClientDisconnectCallback -= DisconnectHandling;
            }
        }

        private void ConnectApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            string nickname = Encoding.Unicode.GetString(request.Payload);

            if(users.Count < 4)
            {
                users.Add(request.ClientNetworkId, nickname);
                Debug.Log(nickname + ":" + request.ClientNetworkId + " Connected");

                response.Approved = true;
                response.CreatePlayerObject = false;
            }
            else
            {
                Debug.Log(nickname + ":" + request.ClientNetworkId + " Approval Failed");

                response.Approved = false;
            }

            Debug.Log("Current User :");

            foreach (var user in users)
            {
                Debug.Log(user.Key + ":" + user.Value);
            }
        }

        private void DisconnectHandling(ulong id)
        {
            isHandlingConnect = true;

            users.Remove(id);

            isHandlingConnect = false;
        }
    }
}
