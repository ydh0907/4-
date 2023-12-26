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

            /*if(isHost) { // 이 값은 server에서 받는다
                HostLobbyPanel(); // 현제는 호스크에서 들어감
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
                if (!isReady) { // 준비 완료를 안함
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

        // 카운터
        private void Counter(Action callback = null) { // 게임 시작시 카운트 다운
            VisualElement counterPanel = countDownPanel.Instantiate().Q<VisualElement>("conuntdown-container");
            var countText = counterPanel.Q<Label>("count-txt");
            _container.Add(counterPanel);

            _counter.CountDown(countText, callback);
        }

        private void ResurrectionCounter(Action callback = null) { // 부활 카운트 다운
            VisualElement counterPanel = deadCountDownPanel.Instantiate().Q<VisualElement>("resurrection-container");
            var countText = counterPanel.Q<Label>("dit-txt");
            _container.Add(counterPanel);

            _counter.ResurrectionCountDown(countText, callback);
        }

        public void FinishCountDown() { // 준비 완료 상태 후 게임 시작 대기가 종료 
            _container.Clear();

            var template = playPanel.Instantiate().Q<VisualElement>("container");

            // 이곳으로 접근하여 각 플레이어별 데이터를 넣어줌

            var nickname = template.Q<Label>("nickname-txt");
            var drinkIcon = template.Q<VisualElement>("drinkIcon");
            killcount = template.Q<Label>("killCount-txt"); // 값을 계속해서 변경하기 때문에 가지고 있음
            timer = template.Q<Label>("time-txt"); // 값을 계속해서 변경하기 때문에 가지고 있음
            Debug.Log("finish time");
            _counter.PlayTimeCountDown(timer);

            _container.Add(template);
            SoundManager.Instance.Play(ingameBGM, Sound.Bgm);
        }
    }
}
