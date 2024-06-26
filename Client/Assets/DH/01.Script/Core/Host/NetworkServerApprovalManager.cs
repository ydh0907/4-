using System;
using System.Collections.Generic;
using System.Text;
using TestClient;
using Unity.Netcode;

namespace DH
{
    public class NetworkServerApprovalManager : NetworkBehaviour
    {
        public static NetworkServerApprovalManager Instance = null;

        public Dictionary<ulong, PlayerInfo> players => NetworkGameManager.Instance.users;

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

                NetworkManager.Singleton.ConnectionApprovalCallback += ConnectApproval;
                NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectHandling;
                NetworkManager.Singleton.OnClientConnectedCallback += ConnectedCallback;
            }
        }

        private void ConnectedCallback(ulong clientId)
        {
            NetworkGameManager.Instance.SyncPlayerList();
            ReadyObjects.Instance.SetCurrentCharactersClientRpc();
            ReadyObjects.Instance.SetNicknameColorClientRpc();
            Program.Instance.UpdateRoom(DH.NetworkGameManager.GetJoinCode(), ConnectManager.Instance.nickname, players.Count);
            NetworkGameManager.Instance.MakePlayerInstance(clientId);
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

                response.Approved = true;
                response.CreatePlayerObject = false;
            }
            else
            {
                response.Approved = false;
            }

            isHandlingConnect = false;

            Program.Instance.UpdateRoom(DH.NetworkGameManager.GetJoinCode(), ConnectManager.Instance.nickname, players.Count);
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

            NetworkGameManager.Instance.SetValue(id, null);

            ReadyObjects.Instance.SetCurrentCharactersClientRpc();
            ReadyObjects.Instance.SetNicknameColorClientRpc();

            isHandlingConnect = false;

            if (NetworkGameManager.MatchingServerConnection)
                Program.Instance.UpdateRoom(DH.NetworkGameManager.GetJoinCode(), ConnectManager.Instance.nickname, players.Count);
        }
    }
}
