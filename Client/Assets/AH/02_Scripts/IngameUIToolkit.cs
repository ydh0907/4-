using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class IngameUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;
        private TimeCounter _counter;
        private VisualElement _container;

        [Header("CountDownPanels")]
        [SerializeField] private VisualTreeAsset deadCountDownPanel;
        [SerializeField] private VisualTreeAsset countDownPanel;
        [Space]

        [Header("LobbyPanel")]
        [SerializeField] private VisualTreeAsset hostLobbyPanel;
        [SerializeField] private VisualTreeAsset clientLobbyPanel;
        [Space]

        [Header("LobbyPanel")]
        [SerializeField] private VisualTreeAsset basicPanel;
        [SerializeField] private VisualTreeAsset playPanel;
        [Space]

        [Header("Data")]
        public bool isHost = true;
        public string nickName;
        public string drinkName;

        private string _joinCode = "1234"; // �� ���� server���� �޴´�
        private bool isReady = false;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
            _counter = GetComponent<TimeCounter>();
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;
            _container = root.Q<VisualElement>("lobby-container");

            if(isHost) { // �� ���� server���� �޴´�
                InputPlayerData();
                HostLobbyPanel(); // ������ ȣ��ũ���� ��
            }
            else {
                InputPlayerData();
                ClientLobbyPanel();
            }
        }
        // Lobby
        private void HostLobbyPanel() {
            VisualElement hostPanel = hostLobbyPanel.Instantiate().Q<VisualElement>("host-content");
            _container.Add(hostPanel);

            Label joinCode = hostPanel.Q<Label>("joincode-txt");
            joinCode.text = _joinCode;
            hostPanel.Q<Button>("startgame-btn").RegisterCallback<ClickEvent>(HaneldStartGame);
        }
        private void ClientLobbyPanel() {
            VisualElement clientPanel = clientLobbyPanel.Instantiate().Q<VisualElement>("client-content");
            _container.Add(clientPanel);

            clientPanel.Q<Button>("ready-btn").RegisterCallback<ClickEvent>(HandleReadyGame);
        }
        private void InputPlayerData() {
            VisualElement hostPanel = hostLobbyPanel.Instantiate().Q<VisualElement>("host-content");
            _container.Add(hostPanel);

            Label joinCode = hostPanel.Q<Label>("joincode-txt");
            joinCode.text = _joinCode;
            hostPanel.Q<Button>("startgame-btn").RegisterCallback<ClickEvent>(HaneldStartGame);
        }

        private void HaneldStartGame(ClickEvent evt) {
            /*if (isReady) { // ���� �ڵ� ���� �ʿ� => ��� Ŭ���� ���� Ȯ���ؾ���(ȣ��Ʈ ����)
            }*/
            _container.Clear();
            Counter();
        }
        private void HandleReadyGame(ClickEvent evt) { // ���� ������ ���� �ʿ� ���ɼ� ����
            Debug.Log($"reday : {isReady}");
            if(nickName !=null && drinkName != null) { // �� �� ���� �Է� ���� ��
                isReady = true;

            }
        }

        // ī����
        private void Counter() {
            VisualElement counterPanel = countDownPanel.Instantiate().Q<VisualElement>("conuntdown-container");
            var countText = counterPanel.Q<Label>("count-txt");
            _container.Add(counterPanel);

            _counter.CountDown(countText);
        } 
        private void ResurrectionCounter() {
            VisualElement counterPanel = deadCountDownPanel.Instantiate().Q<VisualElement>("resurrection-container");
            var countText = counterPanel.Q<Label>("dit-txt");
            _container.Add(counterPanel);

            _counter.ResurrectionCountDown(countText);
        }

        public void FinishCountDown() { // �غ� �Ϸ� ���� �� ���� ���� ��Ⱑ ���� 
            Debug.Log("finish");
        }
    }
}
