using DH;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH {
    public class IngameUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;
        private TimeCounter _counter;
        public VisualElement _container;

        List<VisualElement> playerData = new List<VisualElement>();
        
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
        [SerializeField] private VisualTreeAsset gameOverPanel;
        [SerializeField] private VisualTreeAsset _settingPanel;
        [Space]

        [Header("Data")]
        public bool isHost = true;
        private Label killcount;
        private Label timer;
        private bool isReady = false;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
            _counter = GetComponent<TimeCounter>();
            isHost = NetworkManager.Singleton.IsHost;
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

            hostPanel.Q<Button>("leaveGame-btn").RegisterCallback<ClickEvent>(HandleLeaveGame);
            hostPanel.Q<Button>("startgame-btn").RegisterCallback<ClickEvent>(HaneldStartGame);
            hostPanel.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(HandleSettingTemplate);
        }
        private void ClientLobbyPanel() {
            VisualElement clientPanel = clientLobbyPanel.Instantiate().Q<VisualElement>("client-content");
            _container.Add(clientPanel);

            clientPanel.Q<Button>("leaveGame-btn").RegisterCallback<ClickEvent>(HandleLeaveGame);
            clientPanel.Q<Button>("ready-btn").RegisterCallback<ClickEvent>(HandleReadyGame);
            clientPanel.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(HandleSettingTemplate);
        }

        private void HandleLeaveGame(ClickEvent evt) {
            NetworkGameManager.Instance.ServerGameEnd();
        }

        private void HandleSettingTemplate(ClickEvent evt) {
            SettingTemplate();
        }

        // setting
        private void SettingTemplate() {
            var template = _settingPanel.Instantiate().Q<VisualElement>("setting-border");

            _container.Add(template);

            var bgmData = template.Q<VisualElement>("bgm-content");
            var effectData = template.Q<VisualElement>("effect-content");

            List<VisualElement> bgmList = new List<VisualElement>();
            List<VisualElement> effectList = new List<VisualElement>();

            GetSoundVisualElementData(bgmList, bgmData); // 생성할 때마다 가져와야 함
            GetSoundVisualElementData(effectList, effectData);
            GetcurrentSoundData(bgmList, effectList);

            template.Q<Button>("close-btn").RegisterCallback<ClickEvent, VisualElement>(HandleCloseButton, template);

            VisualElement bgmValueButton = template.Q<VisualElement>(className: "bgm-content");
            VisualElement effectValueButton = template.Q<VisualElement>(className: "effect-content");
            bgmValueButton.RegisterCallback<ClickEvent>(evt => {
                var btn = evt.target as DataSound;
                if (btn != null) {
                    int index = bgmList.IndexOf(btn);

                    SoundManager.Instance.bgmValue = index;
                    SoundManager.Instance.RegulateSound(Sound.Bgm, index);
                    OnOffImages(bgmList, index);
                }
            });
            effectValueButton.RegisterCallback<ClickEvent>(evt => {
                var btn = evt.target as DataSound;
                if (btn != null) {
                    int index = effectList.IndexOf(btn);

                    SoundManager.Instance.effectValue = index;
                    SoundManager.Instance.RegulateSound(Sound.Effect, index);
                    OnOffImages(effectList, index);
                }
            });
        }
        private void GetSoundVisualElementData(List<VisualElement> list, VisualElement data) {
            if (list.Count > 0) {
                list.Clear();
            }
            for (int i = 1; i < data.childCount; i++) {
                list.Add(data[i]);
            }
        }
        private void GetcurrentSoundData(List<VisualElement> bgmList, List<VisualElement> effectList) {
            OnOffImages(bgmList, SoundManager.Instance.bgmValue);
            OnOffImages(effectList, SoundManager.Instance.effectValue);
        }
        private void OnOffImages(List<VisualElement> bgmList, int index) {
            foreach (VisualElement bgm in bgmList) {
                bgm.RemoveFromClassList("on");
            }
            for (int i = 0; i <= index; i++) {
                bgmList[i].AddToClassList("on");
            }
        }
        private void HandleCloseButton(ClickEvent evt, VisualElement template) {
            _container.Remove(template);
        }

        // lobby
        private void HaneldStartGame(ClickEvent evt) {
            SoundManager.Instance.Play("Effect/Button click");
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
                NetworkGameManager.Instance.UILoadServerRpc();
            }
        }
        private void HandleReadyGame(ClickEvent evt) {
            SoundManager.Instance.Play("Effect/Button click");
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

            Debug.Log($"Ready : {isReady}");
        }

        // 카운터
        public void Counter(Action callback = null) { // 게임 시작시 카운트 다운
            SoundManager.Instance.Clear();

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
        } // 플레이어 부활

        public void FinishCountDown() { // 준비 완료 상태 후 게임 시작 대기가 종료 
            _container.Clear();

            var template = playPanel.Instantiate().Q<VisualElement>("container");

            // 이곳으로 접근하여 각 플레이어별 데이터를 넣어줌
            VisualElement basePlayerData = template.Q<VisualElement>(className: "players-border");
            for(int i = 0; i < basePlayerData.childCount; i++) {
                if (basePlayerData[i].name == "player") {
                    playerData.Add(basePlayerData[i]);
                }
            }

            // playerData 리스트에 4개의 visualelement가 있음
            foreach (var data in playerData) {
                var nickname = data.Q<Label>("nickname-txt");
                var drinkIcon = data.Q<VisualElement>("drinkIcon");
                killcount = data.Q<Label>("killCount-txt"); // 값을 계속해서 변경하기 때문에 가지고 있음
            }

            timer = template.Q<Label>("time-txt"); // 값을 계속해서 변경하기 때문에 가지고 있음

            _counter.PlayTimeCountDown(timer);

            _container.Add(template);
            SoundManager.Instance.Play(ingameBGM, Sound.Bgm);
        }
        public void GameOver() {
            SoundManager.Instance.Clear();
            SoundManager.Instance.Play("Effect/DrumRoll");

            VisualElement template = gameOverPanel.Instantiate().Q<VisualElement>("blackContainer");

            _container.Clear();
            _container.Add(template);

            template.Q<Button>("goLobby").RegisterCallback<ClickEvent>(HandleGoLobby);

            StartCoroutine(DrumRoutine(template));
        }
        IEnumerator DrumRoutine(VisualElement template) {
            yield return new WaitForSeconds(2f);

            SoundManager.Instance.Play("Effect/TaDa");

            template.AddToClassList("fadeOff");
            template.Q<VisualElement>("container").AddToClassList("fadeOff");

        }

        private void HandleGoLobby(ClickEvent evt) {
            NetworkGameManager.Instance.ServerGameEnd();
        }
    }
}
