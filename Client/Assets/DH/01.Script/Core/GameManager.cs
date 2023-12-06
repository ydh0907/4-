using UnityEngine;

namespace DH
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private NetworkHost m_NetworkHost;
        private NetworkClient m_NetworkClient;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;

            m_NetworkHost = GetComponent<NetworkHost>();
            m_NetworkClient = GetComponent<NetworkClient>();

            DontDestroyOnLoad(gameObject);
        }

        public void StartClient()
        {
            m_NetworkClient.StartConnect();
        }

        public void StartHost()
        {
            m_NetworkHost.StartConnect();
        }
    }
}
