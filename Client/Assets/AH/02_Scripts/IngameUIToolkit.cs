using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class IngameUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;
        private TimeCounter _counter;
        private VisualElement _container;

        [SerializeField] private VisualTreeAsset countDownPanel;
        [SerializeField] private VisualTreeAsset hostLobbyPanel;
        [SerializeField] private VisualTreeAsset clientLobbyPanel;

        private bool isHost = true;
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
                HostLobbyPanel(); // 현제는 호스크에서 들어감
            }
            else {
                ClientLobbyPanel();
            }
        }
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


        private void HaneldStartGame(ClickEvent evt) {
            _container.Clear();
            Counter();
        }
        private void HandleReadyGame(ClickEvent evt) {

        }

        private void Counter() {
            VisualElement counterPanel = countDownPanel.Instantiate().Q<VisualElement>("conuntdown-container");
            var countText = counterPanel.Q<Label>("count-txt");
            _container.Add(counterPanel);

            _counter.CountDown(countText); // 값 변경
        }
    }
}
