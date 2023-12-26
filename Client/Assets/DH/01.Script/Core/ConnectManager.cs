using UnityEngine;
using TMPro;
using Unity.Netcode;

namespace DH
{
    public class ConnectManager : MonoBehaviour
    {
        public static ConnectManager Instance;

        private NetworkHost m_NetworkHost;
        private NetworkClient m_NetworkClient;

        public string nickname = "Unknown";
        public Cola cola = Cola.Cola;
        public Character character = Character.Beach;

        [SerializeField] public bool host;

        private void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            Instance = this;

            m_NetworkHost = GetComponent<NetworkHost>();
            m_NetworkClient = GetComponent<NetworkClient>();

            DontDestroyOnLoad(gameObject);
        }

        public void StartClient(string Address, string nickname)
        {
            this.nickname = nickname;

            if (nickname.Length < 1) nickname = "Unknown";

            m_NetworkClient.StartConnect(Address);
        }

        public void StartHost(string nickname)
        {
            this.nickname = nickname;

            if (nickname.Length < 1) nickname = "Unknown";

            m_NetworkHost.StartConnect();
        }
    }
}
