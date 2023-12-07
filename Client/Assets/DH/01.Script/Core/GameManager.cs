using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DH
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private NetworkHost m_NetworkHost;
        private NetworkClient m_NetworkClient;

        public string nickname = "Unknown";
        public string IP = "127.0.0.1";

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
            nickname = GameObject.Find("NicknameInput").GetComponent<TMP_InputField>().text;
            IP = GameObject.Find("IPInput").GetComponent<TMP_InputField>().text;

            if (nickname.Length < 1) nickname = "Unknown";

            m_NetworkClient.StartConnect();
        }

        public void StartHost()
        {
            nickname = GameObject.Find("NicknameInput").GetComponent<TMP_InputField>().text;
            IP = "127.0.0.1";

            if (nickname.Length < 1) nickname = "Unknown";

            m_NetworkHost.StartConnect();
        }

        public string GetUserName()
        {
            return nickname;
        }
    }
}
