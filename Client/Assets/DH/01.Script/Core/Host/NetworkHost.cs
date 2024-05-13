using System;
using TestClient;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;

namespace DH
{
    public class NetworkHost : MonoBehaviour
    {
        public static NetworkHost Instance = null;

        [SerializeField] private GameObject NetworkGameManager;
        [SerializeField] private GameObject Map;

        private bool? isConnect = null;
        public bool IsConnect => NetworkManager.Singleton.IsHost;

        public UnityEvent onConnectFailed = new();
        public UnityEvent onConnectSucceed = new();
        public UnityEvent onDisconnected = new();

        public string JoinCode;
        private Allocation allocation;

        public async void StartConnect()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += HostApproval;

            while (true)
            {
                try
                {
                    allocation = await Relay.Instance.CreateAllocationAsync(4);
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    continue;
                }
            }

            while (true)
            {
                try
                {
                    JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
                    Debug.Log(JoinCode);
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    continue;
                }
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            RelayServerData relayData = new RelayServerData(allocation, "dtls");
            transport.SetRelayServerData(relayData);

            isConnect = NetworkManager.Singleton.StartHost();

            if (isConnect == true) OnConnected();
            else OnConnectFailed();
        }

        private void HostApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = false;

            NetworkManager.Singleton.ConnectionApprovalCallback -= HostApproval;
        }

        private void OnConnected()
        {
            isConnect = null;

            LoadSceneManager.Instance.LoadSceneAsync(3, () =>
            {
                Instantiate(NetworkGameManager).GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
                Instantiate(Map).GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
                DH.NetworkGameManager.Instance.MakePlayerInstance(NetworkManager.Singleton.LocalClientId);
            });

            Program.Instance.CreateRoom(DH.NetworkGameManager.GetJoinCode(), ConnectManager.Instance.nickname);
            DH.NetworkGameManager.MatchingServerConnection = true;
            onConnectSucceed?.Invoke();
        }

        private void OnConnectFailed()
        {
            isConnect = null;

            onConnectFailed?.Invoke();
        }
    }
}
