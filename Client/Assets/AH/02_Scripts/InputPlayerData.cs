using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH {
    public class InputPlayerData : MonoBehaviour {
        private UIDocument _uiDocument;
        //private TextField _txtNickname; // 닉네임
        [SerializeField] private VisualTreeAsset createRoomTemplate;

        [SerializeField] private List<Button> drinksList = new List<Button>();
        [SerializeField] private List<Button> playerList = new List<Button>();

        VisualElement root;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
        }
        private void OnEnable() {
            root = _uiDocument.rootVisualElement;

            //_txtNickname = root.Q<TextField>("nickname-inputfeld");
            root.Q<Button>("backToTitle-btn").RegisterCallback<ClickEvent>(HandleBackTitleScene);
            root.Q<Button>("create-btn").RegisterCallback<ClickEvent>(HandleCreateRoom);
            root.Q<Button>("findroom-btn").RegisterCallback<ClickEvent>(HandleFindRoom);

            #region list값 넣기
            VisualElement drinkButtonRow = root.Q<VisualElement>(className: "drink-content");
            for(int i = 0; i < drinkButtonRow.childCount; i++) {
                if(drinkButtonRow[i] as Button != null) {
                    drinksList.Add(drinkButtonRow[i] as Button);
                }
            }
            VisualElement playerButtonRow = root.Q<VisualElement>(className: "player-content");
            for (int i = 0; i < playerButtonRow.childCount; i++) {
                if (playerButtonRow[i] as Button != null) {
                    playerList.Add(playerButtonRow[i] as Button);
                }
            }
            #endregion
            drinkButtonRow.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    ClearToButtonList(drinksList, dve);
                }
            });
            playerButtonRow.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    ClearToButtonList(playerList, dve);
                }
            });
        }

        private void ClearToButtonList(List<Button> list, Button dve) {
            foreach (var button in list) {
                button.RemoveFromClassList("notChoose");
                if (button != dve) {
                    button.AddToClassList("notChoose");
                }
            }
        }

        private void HandleCreateRoom(ClickEvent evt) {
            SceneManager.LoadScene("Ingame"); // host
        }
        private void HandleFindRoom(ClickEvent evt) {
            var template = createRoomTemplate.Instantiate().Q<VisualElement>("container");

            root.Clear();
            root.Add(template);
        }
        private void HandleBackTitleScene(ClickEvent evt) { // host
            SceneManager.LoadScene("Title");
        }
    }
}
