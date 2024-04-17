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
            Debug.Log(Address);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = NetworkServerApprovalManager.WriteApprovalData(new PlayerInfo(ConnectManager.Instance.nickname, ConnectManager.Instance.cola, ConnectManager.Instance.character));

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(Address, (ushort)9070);

            isConnect = NetworkManager.Singleton.StartClient();

            StartCoroutine(WaitConnect());
        }

        private IEnumerator WaitConnect()
        {
            yield return new WaitWhile(() => isConnect == null);

            if (isConnect == true)
            {
                OnConnected();
            }
            else OnConnectFailed();
        }

        private void OnConnected()
        {
            Debug.Log("Connected");

            isConnect = null;

            onConnectSucceed?.Invoke();

            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
        }

        private void OnConnectFailed()
        {
            Debug.Log("ConnectFailed");

            isConnect = null;

            onConnectFailed?.Invoke();
        }

        private void OnDisconnected(ulong id)
        {
            isConnect = null;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            NetworkGameManager.Instance.ServerGameEnd();

            onDisconnected?.Invoke();
        }
    }
}
