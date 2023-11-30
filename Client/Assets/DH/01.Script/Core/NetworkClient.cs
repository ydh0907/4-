using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkClient : MonoBehaviour
    {
        public static NetworkClient Instance = null;

        private bool? isConnect = null;
        public bool IsConnect => NetworkManager.Singleton.IsConnectedClient;

        public Action OnConnectFailed = null;
        public Action OnConnectSucceed = null;

        private void Awake()
        {
            if (Instance != null) enabled = false;
            Instance = this;
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
                OnConnectSucceed?.Invoke();
                OnConnected();
            }
            else OnConnectFailed.Invoke();
        }

        private void OnConnected()
        {
            isConnect = null;
            LoadSceneManager.Instance.LoadScene(1);
        }
    }
}
