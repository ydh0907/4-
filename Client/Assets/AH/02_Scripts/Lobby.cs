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
            _waringTxt = root.Q<Label>("waring-txt");

            _inputJoinCode.RegisterCallback<ChangeEvent<string>>(HandleJoinCodeChanged); // �Է��ϴ� ���� ����� ������ ȣ���
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
            if(_joincode != null && _joincode == _testCode) { // ���⿡ �������� ���� joincode ���� ���ؼ� �� �� �ְ� ���ش�
                Debug.Log("enter");
            }
            else {
                StartCoroutine(DisplayWaring());
                Debug.Log("���� �ٽ� �Է����ּ���..");
            }
        }
        private void OpenSettingWindow(ClickEvent evt) { // setting
            Debug.Log("setting");
        }
        private void ExitGame(ClickEvent evt) { // exit
            Application.Quit();
        }

        IEnumerator DisplayWaring() {
            _waringTxt.text = "�ڵ尡 ���� �ʽ��ϴ�. �ٽ� �ۼ����ּ���";
            yield return new WaitForSeconds(2f);
            _waringTxt.text = "";
            yield return null;
        }
    }
}
