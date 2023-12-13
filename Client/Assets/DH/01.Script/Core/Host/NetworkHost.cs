using System;
using System.Collections;
using Unity.Netcode;
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
        private bool connectFlag = false;
        public bool IsConnect => NetworkManager.Singleton.IsHost;

        public UnityEvent onConnectFailed = new();
        public UnityEvent onConnectSucceed = new();

        public UnityEvent onDisconnected = new();

        private void Awake()
        {
            if (Instance != null) enabled = false;
            Instance = this;
        }

        private void Update()
        {
            if (isConnect == null && connectFlag && !IsConnect)
            {
                OnDisconnected();
            }

            connectFlag = IsConnect;
        }

        public void StartConnect()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += HostApproval;

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
            connectFlag = true;
            LoadSceneManager.Instance.LoadScene(1, () =>
            {
                Instantiate(NetworkGameManager, Vector3.zero, Quaternion.identity).GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            });

            onConnectSucceed?.Invoke();
        }

        private void OnConnectFailed()
        {
            isConnect = null;
            connectFlag = false;

            onConnectFailed?.Invoke();
        }

        private void OnDisconnected()
        {
            isConnect = null;
            connectFlag = false;

            if(SceneManager.GetActiveScene().buildIndex > 0)
                LoadSceneManager.Instance.LoadScene(0);

            onDisconnected?.Invoke();
        }
    }
}
