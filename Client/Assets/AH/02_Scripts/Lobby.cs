using Codice.CM.Client.Differences;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class Lobby : MonoBehaviour {
        private UIDocument _uiDocument;

        private TextField _inputJoinCode;

        private Button _createGame;
        private Button _enterGame;
        private Button _settingWindow;
        private Button _exit;

        private string _joincode;
        [SerializeField] private string _testCode;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;

            _inputJoinCode = root.Q<TextField>("joincode-input");

            _inputJoinCode.RegisterCallback<ChangeEvent<string>>(HandleJoinCodeChanged); // 입력하는 값이 변경될 때마다 호출됨
            root.Q<Button>("create-btn").RegisterCallback<ClickEvent>(HandleCreateRoom);
            root.Q<Button>("enter-btn").RegisterCallback<ClickEvent>(HandleEnterRoom);
            root.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(OpenSettingWindow);
            root.Q<Button>("exit-btn").RegisterCallback<ClickEvent>(ExitGame);
        }

        private void HandleJoinCodeChanged(ChangeEvent<string> evt) { // input joincode
            _joincode = evt.newValue;
        }
        private void HandleCreateRoom(ClickEvent evt) { // host
            Debug.Log("create");
        }
        private void HandleEnterRoom(ClickEvent evt) { // client
            if(_joincode != null && _joincode == _testCode) { // 여기에 서버에서 받은 joincode 값을 비교해서 들어갈 수 있게 해준다
                Debug.Log("enter");
            }
            else {
                Debug.Log("값을 다시 입력해주세요..");
            }
        }
        private void OpenSettingWindow(ClickEvent evt) { // setting
            Debug.Log("setting");
        }
        private void ExitGame(ClickEvent evt) { // exit
            Application.Quit();
        }
    }
}
