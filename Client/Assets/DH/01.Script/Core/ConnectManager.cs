using UnityEngine;

namespace DH
{
    public class ConnectManager : MonoBehaviour
    {
        public static ConnectManager Instance;

        private NetworkHost m_NetworkHost;
        private NetworkClient m_NetworkClient;
        private LoadSceneManager m_LoadSceneManager;

        public string nickname = "Unknown";
        public Cola cola = Cola.Cola;
        public Character character = Character.Beach;

        private void Awake()
        {
            if (Instance != null)
            {
                nickname = Instance.nickname;
                cola = Instance.cola;
                character = Instance.character;
                Destroy(Instance.gameObject);
            }
            Instance = this;
            Debug.Log("Connect Manager Init");

            m_NetworkHost = GetComponent<NetworkHost>();
            m_NetworkClient = GetComponent<NetworkClient>();
            m_LoadSceneManager = GetComponent<LoadSceneManager>();

            NetworkHost.Instance = m_NetworkHost;
            NetworkClient.Instance = m_NetworkClient;
            LoadSceneManager.Instance = m_LoadSceneManager;

            DontDestroyOnLoad(gameObject);

            LoadSceneManager.Instance.LoadScene(1);
        }

        private void OnDestroy()
        {
            Debug.Log("Connect Manager Destroy");
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
