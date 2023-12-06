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
            isConnect = NetworkManager.Singleton.StartHost();

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
            isConnect = null;
            connectFlag = true;
            LoadSceneManager.Instance.LoadScene(1, () =>
            {
                GameObject hostManager = new GameObject("HostManager");
                hostManager.AddComponent<NetworkObject>();
                hostManager.AddComponent<NetworkHostManager>();
                hostManager.AddComponent<NetworkHostConnectManager>();
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
