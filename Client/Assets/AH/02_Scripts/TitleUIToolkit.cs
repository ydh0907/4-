using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH {
    public class TitleUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
        }
        private void OnEnable() {
            var root = _uiDocument.rootVisualElement;

            root.Q<Button>("playgame-btn").RegisterCallback<ClickEvent>(HandleInputPlayerDataScene);
            root.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(OpenSettingWindow);
            root.Q<Button>("exit-btn").RegisterCallback<ClickEvent>(ExitGame);
        }

        private void HandleInputPlayerDataScene(ClickEvent evt) { // host
            SceneManager.LoadScene("InputPlayerData");
        }
        private void OpenSettingWindow(ClickEvent evt) { // setting
            Debug.Log("SETTING");
        }
        private void ExitGame(ClickEvent evt) { // exit
            Application.Quit();
            Debug.Log("QUIT");
        }
    }
}
