using DH;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class IngameUIToolkit : NetworkBehaviour {
        private UIDocument _uiDocument;
        private TimeCounter _counter;
        private VisualElement _container;

        [SerializeField] private AudioClip ingameBGM;

        [Header("CountDownPanels")]
        [SerializeField] private VisualTreeAsset deadCountDownPanel;
        [SerializeField] private VisualTreeAsset countDownPanel;
        [Space]

        [Header("LobbyPanel")]
        [SerializeField] private VisualTreeAsset hostLobbyPanel;
        [SerializeField] private VisualTreeAsset clientLobbyPanel;
        [Space]

        [Header("playPanel")]
        [SerializeField] private VisualTreeAsset playPanel;
        [Space]

        [Header("Data")]
        public bool isHost = true;
        private Label killcount;
        private Label timer;
        private bool isReady = false;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
            _counter = GetComponent<TimeCounter>();
            //isHost = NetworkManager.Singleton.IsHost;
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;
            _container = root.Q<VisualElement>("lobby-container");

            SoundManager.Instance.Init();

            /*if(isHost) { // �� ���� server���� �޴´�
                HostLobbyPanel(); // ������ ȣ��ũ���� ��
            }
            else {
                ClientLobbyPanel();
            }*/
            Counter();
        }
        // Lobby
        private void HostLobbyPanel() {
            VisualElement hostPanel = hostLobbyPanel.Instantiate().Q<VisualElement>("host-content");
            _container.Add(hostPanel);

            hostPanel.Q<Button>("startgame-btn").RegisterCallback<ClickEvent>(HaneldStartGame);
        }
        private void ClientLobbyPanel() {
            VisualElement clientPanel = clientLobbyPanel.Instantiate().Q<VisualElement>("client-content");
            _container.Add(clientPanel);

            var ready = clientPanel.Q<Button>("ready-btn");
            ready.RegisterCallback<ClickEvent>(HandleReadyGame);
        }

        private void HaneldStartGame(ClickEvent evt) {
            if (!NetworkManager.Singleton.IsHost) return;

            bool start = true;

            foreach(var player in NetworkGameManager.Instance.players)
            {
                start = start && player.Value.Ready;
            }

            if(start)
            {
                _container.Clear();
                Counter(NetworkGameManager.Instance.ServerGameStart);


                if(IsHost)
                    HandleStartGameClientRpc();
            }
        }

        [ClientRpc]
        private void HandleStartGameClientRpc()
        {
            Debug.Log("clientCounter");
            _container.Clear();
            Counter();
        }

        private void HandleReadyGame(ClickEvent evt) {
            var dve = evt.target as Button;
            if (dve != null) {
                if (!isReady) { // �غ� �ϷḦ ����
                    isReady = true;
                    NetworkGameManager.Instance.PlayerReadyServerRpc(NetworkManager.Singleton.LocalClientId, isReady);
                    dve.AddToClassList("isReady");
                }
                else {
                    isReady = false;
                    NetworkGameManager.Instance.PlayerReadyServerRpc(NetworkManager.Singleton.LocalClientId, isReady);
                    dve.RemoveFromClassList("isReady");
                }
            }

            Debug.Log($"reday : {isReady}");
        }

        // ī����
        private void Counter(Action callback = null) { // ���� ���۽� ī��Ʈ �ٿ�
            VisualElement counterPanel = countDownPanel.Instantiate().Q<VisualElement>("conuntdown-container");
            var countText = counterPanel.Q<Label>("count-txt");
            _container.Add(counterPanel);

            _counter.CountDown(countText, callback);
        }

        private void ResurrectionCounter(Action callback = null) { // ��Ȱ ī��Ʈ �ٿ�
            VisualElement counterPanel = deadCountDownPanel.Instantiate().Q<VisualElement>("resurrection-container");
            var countText = counterPanel.Q<Label>("dit-txt");
            _container.Add(counterPanel);

            _counter.ResurrectionCountDown(countText, callback);
        }

        public void FinishCountDown() { // �غ� �Ϸ� ���� �� ���� ���� ��Ⱑ ���� 
            _container.Clear();

            var template = playPanel.Instantiate().Q<VisualElement>("container");

            // �̰����� �����Ͽ� �� �÷��̾ �����͸� �־���

            var nickname = template.Q<Label>("nickname-txt");
            var drinkIcon = template.Q<VisualElement>("drinkIcon");
            killcount = template.Q<Label>("killCount-txt"); // ���� ����ؼ� �����ϱ� ������ ������ ����
            timer = template.Q<Label>("time-txt"); // ���� ����ؼ� �����ϱ� ������ ������ ����
            Debug.Log("finish time");
            _counter.PlayTimeCountDown(timer);

            _container.Add(template);
            SoundManager.Instance.Play(ingameBGM, Sound.Bgm);
        }
    }
}
