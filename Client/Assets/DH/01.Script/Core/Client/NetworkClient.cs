using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
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

        private void Awake()
        {
            if (Instance != null) enabled = false;
            Instance = this;
        }

        public void StartConnect(string Address)
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = NetworkServerApprovalManager.WriteApprovalData(new PlayerInfo(ConnectManager.Instance.nickname, ConnectManager.Instance.cola, ConnectManager.Instance.character));

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(Address, (ushort)9070, "0.0.0.0");
            
            isConnect = NetworkManager.Singleton.StartClient();

            StartCoroutine(WaitConnect());
        }

        private IEnumerator WaitConnect()
        {
            yield return new WaitWhile(() => isConnect == null);

            if(isConnect == true)
            {
                OnConnected();
            }
            else OnConnectFailed();
        }

        private void OnConnected()
        {
            isConnect = null;

            ConnectManager.Instance.host = NetworkManager.Singleton.IsHost;

            onConnectSucceed?.Invoke();
        }

        private void OnConnectFailed()
        {
            isConnect = null;

            onConnectFailed?.Invoke();
        }

        private void OnDisconnected()
        {
            isConnect = null;

            onDisconnected?.Invoke();
        }
    }
}
