using Karin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace DH {
    public class TitleUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;
        private VisualElement root;

        [SerializeField] private VisualTreeAsset _settingPanel;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
        }
        private void OnEnable() {
            root = _uiDocument.rootVisualElement;

            root.Q<Button>("playgame-btn").RegisterCallback<ClickEvent>(HandleInputPlayerDataScene);
            root.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(OpenSettingWindow);
            root.Q<Button>("exit-btn").RegisterCallback<ClickEvent>(ExitGame);
        }

        private void HandleInputPlayerDataScene(ClickEvent evt) { // host
            SceneManager.LoadScene("DH_Lobby");
        }
        private void OpenSettingWindow(ClickEvent evt) { // setting
            var template = _settingPanel.Instantiate().Q<VisualElement>("setting-border");
            var bgmData = template.Q<VisualElement>("bgm-content");
            var effectData = template.Q<VisualElement>("effect-content");

            List<VisualElement> bgmList = new List<VisualElement>();
            List<VisualElement> effectList = new List<VisualElement>();

            //GetSoundVisualElementData(bgmList, bgmData); // 생성할 때마다 가져와야 함
            //GetSoundVisualElementData(effectList, effectData);
            //GetcurrentSoundData(bgmList, effectList);

            root.Clear();
            root.Add(template);

            template.AddToClassList("on");

            template.Q<Button>("close-btn").RegisterCallback<ClickEvent>(HandleCloseButton);

            VisualElement bgmValueButton = template.Q<VisualElement>(className: "bgm-content");
            VisualElement effectValueButton = template.Q<VisualElement>(className: "effect-content");
            bgmValueButton.RegisterCallback<ClickEvent>(evt => {
                var btn = evt.target as DataSound;
                if (btn != null) {
                    int index = bgmList.IndexOf(btn);

                    //SoundManager.Instance.bgmValue = index;
                    //SoundManager.Instance.RegulateSound(Sound.Bgm, index);
                    OnOffImages(bgmList, index);
                }
            });
            effectValueButton.RegisterCallback<ClickEvent>(evt => {
                var btn = evt.target as DataSound;
                if (btn != null) {
                    int index = effectList.IndexOf(btn);

                    //SoundManager.Instance.effectValue = index;
                    //SoundManager.Instance.RegulateSound(Sound.Effect, index);
                    OnOffImages(effectList, index);
                }
            });
        }
        private void HandleCloseButton(ClickEvent evt) {

        }

        private void ExitGame(ClickEvent evt) { // exit
            Application.Quit();
            Debug.Log("QUIT");
        }
    }
}
