using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkServerApprovalManager : NetworkBehaviour
    {
        public static NetworkServerApprovalManager Instance = null;

        private PlayerDictionary<PlayerInfo> players => NetworkGameManager.Instance.players.Value;

        public bool isHandlingConnect = false;

        public bool ApprovalShutdown = false;

        private void Start()
        {
            Instance = this;

            if (IsHost)
            {
                players.Add(NetworkManager.Singleton.LocalClientId, new PlayerInfo(NetworkManager.Singleton.LocalClientId, ConnectManager.Instance.nickname));

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

            if(players.Count < 4)
            {
                players.Add(request.ClientNetworkId, new PlayerInfo(request.ClientNetworkId, nickname));
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
            Debug.Log("Current User :" + players.Count);
        }

        private void DisconnectHandling(ulong id)
        {
            isHandlingConnect = true;

            players.Remove(id);

            isHandlingConnect = false;
            Debug.Log("Current User :" + players.Count);
        }
    }
}
