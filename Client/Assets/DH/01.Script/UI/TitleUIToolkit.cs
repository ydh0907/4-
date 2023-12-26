using Karin;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace DH {
    public class TitleUIToolkit : MonoBehaviour {
        private UIDocument _uiDocument;
        private VisualElement _root;

        [SerializeField] private VisualTreeAsset _titlePanel;
        [SerializeField] private VisualTreeAsset _settingPanel;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
            A_SoundManager.Instance.Init();
        }
        private void OnEnable() {
            _root = _uiDocument.rootVisualElement;
            _root = _root.Q<VisualElement>("container");

            TitleTemplate();
        }
        private void OpenSettingWindow(ClickEvent evt) { // setting
            SettingTemplate();
        }
        private void TitleTemplate() {
            var template = _titlePanel.Instantiate().Q<VisualElement>("title-container");

            _root.Clear();
            _root.Add(template);

            template.Q<Button>("playgame-btn").RegisterCallback<ClickEvent>(HandleInputPlayerDataScene);
            template.Q<Button>("setting-btn").RegisterCallback<ClickEvent>(OpenSettingWindow);
            template.Q<Button>("exit-btn").RegisterCallback<ClickEvent>(ExitGame);

        }
        private void SettingTemplate() {
            var template = _settingPanel.Instantiate().Q<VisualElement>("setting-border");

            //_root.Clear();
            _root.Add(template);

            var bgmData = template.Q<VisualElement>("bgm-content");
            var effectData = template.Q<VisualElement>("effect-content");

            List<VisualElement> bgmList = new List<VisualElement>();
            List<VisualElement> effectList = new List<VisualElement>();

            //GetSoundVisualElementData(bgmList, bgmData); // ������ ������ �����;� ��
            //GetSoundVisualElementData(effectList, effectData);
            //GetcurrentSoundData(bgmList, effectList);

            template.Q<Button>("close-btn").RegisterCallback<ClickEvent>(HandleCloseButton);

            VisualElement bgmValueButton = template.Q<VisualElement>(className: "bgm-content");
            VisualElement effectValueButton = template.Q<VisualElement>(className: "effect-content");
            bgmValueButton.RegisterCallback<ClickEvent>(evt => {
                var btn = evt.target as DataSound;
                if (btn != null) {
                    int index = bgmList.IndexOf(btn);
                    Debug.Log("click");
                    A_SoundManager.Instance.bgmValue = index;
                    A_SoundManager.Instance.RegulateSound(Sound.Bgm, index);
                    OnOffImages(bgmList, index);
                }
            });
            effectValueButton.RegisterCallback<ClickEvent>(evt => {
                var btn = evt.target as DataSound;
                if (btn != null) {
                    int index = effectList.IndexOf(btn);
                    Debug.Log("click");
                    A_SoundManager.Instance.effectValue = index;
                    A_SoundManager.Instance.RegulateSound(Sound.Effect, index);
                    OnOffImages(effectList, index);
                }
            });
        }

        private void OnOffImages(List<VisualElement> bgmList, int index) {

        }

        private void HandleCloseButton(ClickEvent evt) {
            TitleTemplate();
        }
        private void HandleInputPlayerDataScene(ClickEvent evt) { // host
            SceneManager.LoadScene("DH_Lobby");
        }
        private void ExitGame(ClickEvent evt) { // exit
            Application.Quit();
            Debug.Log("QUIT");
        }
    }
}
