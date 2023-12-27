using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkServerApprovalManager : NetworkBehaviour
    {
        public static NetworkServerApprovalManager Instance = null;

        public Dictionary<ulong, PlayerInfo> players => NetworkGameManager.Instance.players;

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
                players.Add(NetworkManager.Singleton.LocalClientId, new PlayerInfo(NetworkManager.Singleton.LocalClientId, ConnectManager.Instance.nickname, ConnectManager.Instance.cola, ConnectManager.Instance.character, true));

                UserLog();

                NetworkManager.Singleton.ConnectionApprovalCallback += ConnectApproval;
                NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectHandling;
                NetworkManager.Singleton.OnClientConnectedCallback += ConnectedCallback;
            }
        }

        private void ConnectedCallback(ulong obj)
        {
            NetworkGameManager.Instance.SyncPlayerList();
            ReadyObjects.Instance.SetCurrentCharactersClientRpc();
            ReadyObjects.Instance.SetNicknameColorClientRpc();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback -= ConnectApproval;
                NetworkManager.Singleton.OnClientDisconnectCallback -= DisconnectHandling;
                NetworkManager.Singleton.OnClientConnectedCallback -= ConnectedCallback;
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
                players.Add(request.ClientNetworkId, new PlayerInfo(request.ClientNetworkId, info.Nickname, info.Cola, info.Char));

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
            Character character = (Character)BitConverter.ToUInt32(payload, process);
            process += sizeof(int);
            string nickname = Encoding.Unicode.GetString(payload, process, payload.Length - process);

            return new PlayerInfo(NetworkManager.Singleton.LocalClientId, nickname, cola, character);
        }

        public static byte[] WriteApprovalData(PlayerInfo info)
        {
            int process = 0;
            byte[] buffer = new byte[256];

            Buffer.BlockCopy(BitConverter.GetBytes((int)info.Cola), 0, buffer, process, 4);
            process += sizeof(int);
            Buffer.BlockCopy(BitConverter.GetBytes((int)info.Char), 0, buffer, process, 4);
            process += sizeof(int);
            process += Encoding.Unicode.GetBytes(info.Nickname, 0, info.Nickname.Length, buffer, 0 + process);

            byte[] sender = new byte[process];
            Buffer.BlockCopy(buffer, 0, sender, 0, process);

            return sender;
        }

        private void DisconnectHandling(ulong id)
        {
            isHandlingConnect = true;

            players.Remove(id);

            NetworkGameManager.Instance.SetValueServerRpc(id, null);

            ReadyObjects.Instance.SetCurrentCharactersClientRpc();
            ReadyObjects.Instance.SetNicknameColorClientRpc();

            isHandlingConnect = false;

            UserLog();
        }

        public void UserLog()
        {
            string log = "";

            foreach (var player in players)
            {
                log += player.Key.ToString() + " : " + player.Value.Nickname + " : " + player.Value.Cola.ToString() + "\n";
            }

            Debug.Log(log);
        }

        [ClientRpc]
        public void UserLogClientRpc()
        {
            string log = "";

            foreach (var player in players)
            {
                log += player.Key.ToString() + " : " + player.Value.Nickname + " : " + player.Value.Cola.ToString() + "\n";
            }

            Debug.Log(log);
        }
    }
}
