using System;
using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DH
{
    public class NetworkHost : MonoBehaviour
    {
        public static NetworkHost Instance = null;

        [SerializeField] private GameObject NetworkGameManager;

        private bool? isConnect = null;
        public bool IsConnect => NetworkManager.Singleton.IsHost;

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
            NetworkManager.Singleton.ConnectionApprovalCallback += HostApproval;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.0", (ushort)9070, "0.0.0.0");

            isConnect = NetworkManager.Singleton.StartHost();

            StartCoroutine(WaitConnect());
        }

        private void HostApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = false;

            NetworkManager.Singleton.ConnectionApprovalCallback -= HostApproval;
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
            isConnect = null;

            LoadSceneManager.Instance.LoadScene(2, () => Instantiate(NetworkGameManager).GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId));

            onConnectSucceed?.Invoke();
        }

        private void OnConnectFailed()
        {
            isConnect = null;

            onConnectFailed?.Invoke();
        }
    }
}
