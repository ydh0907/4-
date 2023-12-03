using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DH
{
    public class NetworkClient : MonoBehaviour
    {
        public static NetworkClient Instance = null;

        private bool? isConnect = null;
        private bool connectFlag = false;
        public bool IsConnect => NetworkManager.Singleton.IsConnectedClient;

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
            if(isConnect == null && connectFlag && !IsConnect)
            {
                OnDisconnected();
            }

            connectFlag = IsConnect;
        }

        public void StartConnect()
        {
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
            connectFlag = true;
            LoadSceneManager.Instance.LoadScene(1);

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

            onDisconnected?.Invoke();
        }
    }
}
