using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;

namespace DH
{
    public class NetworkClient : MonoBehaviour
    {
        public static NetworkClient Instance = null;

        private bool? isConnect = null;
        public bool IsConnect => NetworkManager.Singleton.IsConnectedClient;

        public UnityEvent onConnectFailed = new();
        public UnityEvent onConnectSucceed = new();
        public UnityEvent onDisconnected = new();

        private JoinAllocation allocation;

        public async void StartConnect(string joinCode)
        {
            Debug.Log(joinCode);

            LoadingCanvasSingleton.Singleton.SetStateSceneLoader(true);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = NetworkServerApprovalManager.WriteApprovalData(new PlayerInfo(ConnectManager.Instance.nickname, ConnectManager.Instance.cola, ConnectManager.Instance.character));

            try
            {
                allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
                transport.SetRelayServerData(relayServerData);

                isConnect = NetworkManager.Singleton.StartClient();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                isConnect = false;
            }

            if (isConnect == true)
            {
                OnConnected();
            }
            else OnConnectFailed();
        }

        private void OnConnected()
        {
            isConnect = null;
            onConnectSucceed?.Invoke();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
        }

        private void OnConnectFailed()
        {
            LoadingCanvasSingleton.Singleton.SetStateSceneLoader(false);
            isConnect = null;
            onConnectFailed?.Invoke();
        }

        private void OnDisconnected(ulong id)
        {
            isConnect = null;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            LoadSceneManager.Instance.LoadScene(2);
            onDisconnected?.Invoke();
        }
    }
}
