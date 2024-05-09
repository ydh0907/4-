using Unity.Services.Authentication;
using UnityEngine;

namespace DH
{
    public class ConnectManager : MonoBehaviour
    {
        public static ConnectManager Instance;

        private NetworkHost m_NetworkHost;
        private NetworkClient m_NetworkClient;
        private LoadSceneManager m_LoadSceneManager;
        private AuthManager m_AuthManager;
        private LobbyManager m_LobbyManager;

        public string nickname = "Nickname";
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

            m_NetworkHost = GetComponent<NetworkHost>();
            m_NetworkClient = GetComponent<NetworkClient>();
            m_LoadSceneManager = GetComponent<LoadSceneManager>();
            m_AuthManager = GetComponent<AuthManager>();
            m_LobbyManager = GetComponent<LobbyManager>();

            NetworkHost.Instance = m_NetworkHost;
            NetworkClient.Instance = m_NetworkClient;
            LoadSceneManager.Instance = m_LoadSceneManager;
            AuthManager.Instance = m_AuthManager;
            LobbyManager.Instance = m_LobbyManager;

            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            await AuthManager.Instance.Authorization();
            Debug.Log(AuthenticationService.Instance.IsAuthorized && AuthenticationService.Instance.IsSignedIn);
            Debug.Log(AuthenticationService.Instance.PlayerId);
            LoadSceneManager.Instance.LoadSceneAsync(1);
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
