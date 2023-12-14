using System.Collections;
using System.Text;
using Unity.Netcode;
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

        public void StartConnect()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = NetworkServerApprovalManager.WriteApprovalData(new PlayerInfo());

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
