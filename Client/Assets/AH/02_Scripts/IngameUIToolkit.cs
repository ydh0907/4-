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

        private string _joinCode = "1234"; // 이 값은 server에서 받는다
        private bool isReady = false;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
            _counter = GetComponent<TimeCounter>();
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;
            _container = root.Q<VisualElement>("lobby-container");

            if(isHost) { // 이 값은 server에서 받는다
                InputPlayerData();
                HostLobbyPanel(); // 현제는 호스크에서 들어감
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
            /*if (isReady) { // 여기 코드 수정 필요 => 모든 클라의 값을 확인해야함(호스트 제외)
            }*/
            _container.Clear();
            Counter();
        }
        private void HandleReadyGame(ClickEvent evt) { // 서버 들어오고 수정 필요 가능서 있음
            Debug.Log($"reday : {isReady}");
            if(nickName !=null && drinkName != null) { // 둘 다 값을 입력 했을 때
                isReady = true;

            }
        }

        // 카운터
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

        public void FinishCountDown() { // 준비 완료 상태 후 게임 시작 대기가 종료 
            Debug.Log("finish");
        }
    }
}
