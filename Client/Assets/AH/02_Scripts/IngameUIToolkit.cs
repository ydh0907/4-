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
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;
            _container = root.Q<VisualElement>("lobby-container");

            if(isHost) { // 이 값은 server에서 받는다
                HostLobbyPanel(); // 현제는 호스크에서 들어감
            }
            else {
                ClientLobbyPanel();
            }
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
            /*if (isReady) { // 여기 코드 수정 필요 => 모든 클라의 값을 확인해야함(호스트 제외)
            }*/
            _container.Clear();
            Counter();
        }
        private void HandleReadyGame(ClickEvent evt) {
            var dve = evt.target as Button;
            if (dve != null) {
                if (!isReady) { // 준비 완료를 안함
                    isReady = true;
                    dve.AddToClassList("isReady");
                }
                else {
                    isReady = false;
                    dve.RemoveFromClassList("isReady");
                }
            }

            Debug.Log($"reday : {isReady}");
        }

        // 카운터
        private void Counter() { // 게임 시작시 카운트 다운
            VisualElement counterPanel = countDownPanel.Instantiate().Q<VisualElement>("conuntdown-container");
            var countText = counterPanel.Q<Label>("count-txt");
            _container.Add(counterPanel);

            _counter.CountDown(countText);
        }
        private void ResurrectionCounter() { // 부활 카운트 다운
            VisualElement counterPanel = deadCountDownPanel.Instantiate().Q<VisualElement>("resurrection-container");
            var countText = counterPanel.Q<Label>("dit-txt");
            _container.Add(counterPanel);

            _counter.ResurrectionCountDown(countText);
        }

        public void FinishCountDown() { // 준비 완료 상태 후 게임 시작 대기가 종료 
            _container.Clear();

            var template = playPanel.Instantiate().Q<VisualElement>("container");

            // 이곳으로 접근하여 각 플레이어별 데이터를 넣어줌

            var nickname = template.Q<Label>("nickname-txt");
            var drinkIcon = template.Q<VisualElement>("drinkIcon");
            killcount = template.Q<Label>("killCount-txt"); // 값을 계속해서 변경하기 때문에 가지고 있음
            timer = template.Q<Label>("time-txt"); // 값을 계속해서 변경하기 때문에 가지고 있음

            _counter.PlayTimeCountDown(timer);

            _container.Add(template);
        }
    }
}
