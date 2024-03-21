using DH;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class CustomPlayerAndDrinkData : UI {
        [SerializeField] protected VisualTreeAsset chooseCustomizingTemplate;

        protected List<Button> drinksList = new List<Button>();
        protected List<Button> playerList = new List<Button>();

        public void OnCustomizingTemplate(VisualElement _dataBorder) {
            var template = chooseCustomizingTemplate.Instantiate().Q<VisualElement>("choose-player-or-drink-border");

            _dataBorder.Add(template);

            root.Q<Button>("customizing-player-btn").RegisterCallback<ClickEvent>(HandleGoToChoosePlayer);
            root.Q<Button>("customizing-drink-btn").RegisterCallback<ClickEvent>(HandleGoToChooseDrink);

        }
        private void HandleGoToChooseDrink(ClickEvent evt) {

        }
        private void HandleGoToChoosePlayer(ClickEvent evt) {

        }
        private void ClickDrinkButton() {
            VisualElement drinkButtonRow = root.Q<VisualElement>(className: "drink-content");
            _lobbyMain.GetInputListData(drinkButtonRow, drinksList);

            drinkButtonRow.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    _lobbyMain.ClearToButtonList(drinksList, dve);
                    switch (dve.name) {
                        case "cola-btn": {
                                ConnectManager.Instance.cola = Cola.Cola;
                                break;
                            }
                        case "pinapple-btn": {
                                ConnectManager.Instance.cola = Cola.Pineapple;
                                break;
                            }
                        case "cider-btn": {
                                ConnectManager.Instance.cola = Cola.Sprite;
                                break;
                            }
                        case "orangi-btn": {
                                ConnectManager.Instance.cola = Cola.Orange;
                                break;
                            }
                    }
                }
            });
        }
        private void ClickPlayerButton() {
            VisualElement playerButtonRow = root.Q<VisualElement>(className: "player-content");
            _lobbyMain.GetInputListData(playerButtonRow, playerList);

            playerButtonRow.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    _lobbyMain.ClearToButtonList(playerList, dve);
                    switch (dve.name) {
                        case "BeachGuy": {
                                ConnectManager.Instance.character = Character.Beach;
                                break;
                            }
                        case "AmericanFootballer": {
                                ConnectManager.Instance.character = Character.Football;
                                break;
                            }
                        case "BusinessGuy": {
                                ConnectManager.Instance.character = Character.Business;
                                break;
                            }
                        case "DiscoGuy": {
                                ConnectManager.Instance.character = Character.Disco;
                                break;
                            }
                        case "Farmer": {
                                ConnectManager.Instance.character = Character.Farmer;
                                break;
                            }
                        case "Police": {
                                ConnectManager.Instance.character = Character.Police;
                                break;
                            }
                        case "SoccerGuy": {
                                ConnectManager.Instance.character = Character.Soccer;
                                break;
                            }
                        case "Thief": {
                                ConnectManager.Instance.character = Character.Thief;
                                break;
                            }
                    }
                }
            });
        }
    }
}
