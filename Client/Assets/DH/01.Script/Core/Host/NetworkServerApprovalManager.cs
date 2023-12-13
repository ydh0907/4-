using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkServerApprovalManager : NetworkBehaviour
    {
        public static NetworkServerApprovalManager Instance = null;

        public List<PlayerInfo> players => NetworkGameManager.Instance.players;

        public bool isHandlingConnect = false;

        public bool ApprovalShutdown = false;

        private void Awake()
        {
            Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                players.Add(new PlayerInfo(NetworkManager.Singleton.LocalClientId, ConnectManager.Instance.nickname));
                UserLog();

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
            if (ApprovalShutdown)
            {
                response.Approved = false;
                return;
            }

            isHandlingConnect = true;

            string nickname = Encoding.Unicode.GetString(request.Payload);

            if (players.Count < 4)
            {
                players.Add(new PlayerInfo(request.ClientNetworkId, nickname));
                Debug.Log(nickname + ":" + request.ClientNetworkId + " Connected");

                response.Approved = true;
                response.CreatePlayerObject = false;
            }

            else
            {
                Debug.Log(nickname + ":" + request.ClientNetworkId + " Approval Failed");

                response.Approved = false;
            }

            isHandlingConnect = false;

            UserLog();
        }

        private void DisconnectHandling(ulong id)
        {
            isHandlingConnect = true;

            foreach(var player in players)
            {
                if(player.ID == id)
                {
                    players.Remove(player);
                }
            }

            isHandlingConnect = false;

            UserLog();
        }

        private void UserLog()
        {
            string log = "";

            foreach (var player in players)
            {
                log += player.ID.ToString() + " : " + player.Nickname + "\n";
            }

            Debug.Log(log);
        }
    }
}
