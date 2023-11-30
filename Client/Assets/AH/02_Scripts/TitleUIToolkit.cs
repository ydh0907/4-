using Codice.CM.Client.Differences;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH {
    public class TitleUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;

        private TextField _inputJoinCode;
        private Label _waringTxt;
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
            Debug.Log("CREATE");
        } // host
        private void HandleEnterRoom(ClickEvent evt) { 
            if(_joincode != null && _joincode == _testCode) { // ���⿡ �������� ���� joincode ���� ���ؼ� �� �� �ְ� ���ش�
                SceneManager.LoadScene("Ingame_AH");
                //Debug.Log("ENTER");
            }
            else {
                StartCoroutine(DisplayWaring());
                Debug.Log("���� �ٽ� �Է����ּ���..");
            }
        } // client
        private void OpenSettingWindow(ClickEvent evt) { // setting
            Debug.Log("SETTING");
        }
        private void ExitGame(ClickEvent evt) { // exit
            Application.Quit();
            Debug.Log("QUIT");
        }

        IEnumerator DisplayWaring() {
            _waringTxt.text = "���� �ٽ� �Է����ּ���";
            yield return new WaitForSeconds(2f);
            _waringTxt.text = "";
            yield return null;
        }
    }
}
