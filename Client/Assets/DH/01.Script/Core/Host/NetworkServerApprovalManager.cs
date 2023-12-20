using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

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
                players.Add(new PlayerInfo(NetworkManager.Singleton.LocalClientId, ConnectManager.Instance.nickname, ConnectManager.Instance.cola));
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

            PlayerInfo info = ReadApprovalData(request.Payload);

            if (players.Count < 4)
            {
                players.Add(new PlayerInfo(request.ClientNetworkId, info.Nickname, info.Cola));
                OnValueChangedClientRpc();

                Debug.Log(info.Nickname + ":" + request.ClientNetworkId + " Connected");

                response.Approved = true;
                response.CreatePlayerObject = false;
            }

            else
            {
                Debug.Log(info.Nickname + ":" + request.ClientNetworkId + " Approval Failed");

                response.Approved = false;
            }

            isHandlingConnect = false;

            UserLog();
        }

        public static PlayerInfo ReadApprovalData(byte[] payload)
        {
            int process = 0;

            Cola cola = (Cola)BitConverter.ToUInt32(payload, process);
            process += sizeof(int);
            string nickname = Encoding.Unicode.GetString(payload, process, payload.Length - process);

            return new PlayerInfo(NetworkManager.Singleton.LocalClientId, nickname, cola);
        }

        public static byte[] WriteApprovalData(PlayerInfo info)
        {
            int process = 0;
            byte[] buffer = new byte[256];

            Buffer.BlockCopy(BitConverter.GetBytes((int)info.Cola), 0, buffer, 0, 4);
            process += sizeof(int);
            process += Encoding.Unicode.GetBytes(info.Nickname, 0, info.Nickname.Length, buffer, 0 + process);

            byte[] sender = new byte[process];
            Buffer.BlockCopy(buffer, 0, sender, 0, process);

            return sender;
        }

        private void DisconnectHandling(ulong id)
        {
            isHandlingConnect = true;

            foreach(var player in players)
            {
                if (player.ID == OwnerClientId) return;

                if(player.ID == id)
                {
                    players.Remove(player);
                    OnValueChangedClientRpc();
                }
            }

            isHandlingConnect = false;

            UserLog();
        }

        [ClientRpc]
        private void OnValueChangedClientRpc()
        {

        }

        private void UserLog()
        {
            string log = "";

            foreach (var player in players)
            {
                log += player.ID.ToString() + " : " + player.Nickname + " : " + player.Cola.ToString() + "\n";
            }

            Debug.Log(log);
        }
    }
}
