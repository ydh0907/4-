using Codice.CM.Client.Differences;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class Lobby : MonoBehaviour {
        private UIDocument _uiDocument;

        private TextField _inputJoinCode;

        private Label _waringTxt;

        //private Button _createGame;
        //private Button _enterGame;
        //private Button _settingWindow;
        //private Button _exit;

        private string _joincode;
        [SerializeField] private string _inputCode;


        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;

            _inputJoinCode = root.Q<TextField>("joincode-input");
            _waringTxt = root.Q<Label>("waring-txt");

            _inputJoinCode.RegisterCallback<ChangeEvent<string>>(HandleJoinCodeChanged); // 입력하는 값이 변경될 때마다 호출됨

            root.Q<Button>("create-btn").RegisterCallback<ClickEvent>(HandleCreateRoom);
            root.Q<Button>("enter-btn").RegisterCallback<ClickEvent>(HandleEnterRoom);
            root.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(OpenSettingWindow);
            root.Q<Button>("exit-btn").RegisterCallback<ClickEvent>(ExitGame);
        }

        /// <summary>
        /// input joincode(값이 변경될 때마다 호출됨
        /// </summary>
        /// <param name="evt"></param>
        private void HandleJoinCodeChanged(ChangeEvent<string> evt) {
            _joincode = evt.newValue;
        }
        private void HandleCreateRoom(ClickEvent evt) { // host
            Debug.Log("CREATE ROOM");
        }
        private void HandleEnterRoom(ClickEvent evt) { // client
            if(_joincode != null && _joincode == _inputCode) { // 여기에 서버에서 받은 joincode 값을 비교해서 들어갈 수 있게 해준다
                Debug.Log("ENTER ROOM");
            }
            else {
                StartCoroutine(DisplayWaring());
            }
        }
        private void OpenSettingWindow(ClickEvent evt) { // setting
            Debug.Log("SETTING");
        }
        private void ExitGame(ClickEvent evt) { // exit
            Debug.Log("EXIT");
            Application.Quit();
        }

        IEnumerator DisplayWaring() {
            _waringTxt.text = "코드가 맞지 않습니다.\n다시 작성해주세요";
            yield return new WaitForSeconds(2f);
            _waringTxt.text = "";
            yield return null;
        }
    }
}
