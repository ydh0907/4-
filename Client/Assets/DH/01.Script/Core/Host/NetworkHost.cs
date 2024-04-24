using TestClient;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
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

        public void StartConnect()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += HostApproval;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", (ushort)9070, "0.0.0.0");

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

            Program.Instance.CreateRoom(DH.NetworkGameManager.GetLocalIP(), ConnectManager.Instance.nickname);
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
