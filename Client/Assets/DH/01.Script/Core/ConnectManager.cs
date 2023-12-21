using UnityEngine;
using TMPro;

namespace DH
{
    public class ConnectManager : MonoBehaviour
    {
        public static ConnectManager Instance;

        private NetworkHost m_NetworkHost;
        private NetworkClient m_NetworkClient;

        public string nickname = "Unknown";
        public Cola cola = Cola.Cola;
        public Char character = Char.Beach;

        private void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            Instance = this;

            m_NetworkHost = GetComponent<NetworkHost>();
            m_NetworkClient = GetComponent<NetworkClient>();

            DontDestroyOnLoad(gameObject);
        }

        public void StartClient(string Address)
        {
            nickname = GameObject.Find("NicknameInput").GetComponent<TMP_InputField>().text;

            if (nickname.Length < 1) nickname = "Unknown";

            m_NetworkClient.StartConnect(Address);
        }

        public void StartHost()
        {
            nickname = GameObject.Find("NicknameInput").GetComponent<TMP_InputField>().text;

            if (nickname.Length < 1) nickname = "Unknown";

            m_NetworkHost.StartConnect();
        }
    }
}
