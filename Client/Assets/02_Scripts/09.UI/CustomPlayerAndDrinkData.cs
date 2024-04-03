using DH;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH
{
    public class CustomPlayerAndDrinkData : UI
    {
        [SerializeField] protected VisualTreeAsset chooseCustomizingTemplate;
        [SerializeField] protected VisualTreeAsset customizingPlayerTemplate;
        [SerializeField] protected VisualTreeAsset customizingDrinkTemplate;
        protected List<Button> drinksList = new List<Button>();
        protected List<Button> playerList = new List<Button>();


        public void HandleGoToChoosePlayer(ClickEvent evt, VisualElement addElement)
        {
            InstantiateTemplate(customizingPlayerTemplate, addElement, "content");

            EnterExitButton(addElement);
            ClickPlayerButton();
        }

        public void HandleGoToChooseDrink(ClickEvent evt, VisualElement addElement)
        {
            InstantiateTemplate(customizingDrinkTemplate, addElement, "content");

            EnterExitButton(addElement);
            ClickDrinkButton();
        }

        private void EnterExitButton(VisualElement addElement)
        {
            root.Q<VisualElement>("choose-this-btn")
                .RegisterCallback<ClickEvent>((e) => GoToCustomPlayerData(e, addElement));
            root.Q<VisualElement>("back-btn").RegisterCallback<ClickEvent>((e) => GoToCustomPlayerData(e, addElement));
            _lobbyMain.GetNickName();
        }

        private void GoToCustomPlayerData(ClickEvent evt, VisualElement addElement)
        {
            InstantiateTemplate(chooseCustomizingTemplate, addElement, "container");
            _lobbyMain.CustomDataSetting();
        }

        private void ClickDrinkButton()
        {
            VisualElement drinkButtonRow = root.Q<VisualElement>(className: "drink-content");
            _lobbyMain.GetInputListData(drinkButtonRow, drinksList);

            drinkButtonRow.RegisterCallback<ClickEvent>(evt =>
            {
                var dve = evt.target as Button;
                if (dve != null)
                {
                    _lobbyMain.ClearToButtonList(drinksList, dve);
                    switch (dve.name)
                    {
                        case "cola-btn":
                        {
                            ConnectManager.Instance.cola = Cola.Cola;
                            break;
                        }
                        case "pinapple-btn":
                        {
                            ConnectManager.Instance.cola = Cola.Pineapple;
                            break;
                        }
                        case "cider-btn":
                        {
                            ConnectManager.Instance.cola = Cola.Sprite;
                            break;
                        }
                        case "orangi-btn":
                        {
                            ConnectManager.Instance.cola = Cola.Orange;
                            break;
                        }
                    }


                }
            });
        }

        private void ClickPlayerButton()
        {
            VisualElement playerButtonRow = root.Q<VisualElement>(className: "player-content");
            _lobbyMain.GetInputListData(playerButtonRow, playerList);

            playerButtonRow.RegisterCallback<ClickEvent>(evt =>
            {
                var dve = evt.target as Button;
                if (dve != null)
                {
                    _lobbyMain.ClearToButtonList(playerList, dve);
                    switch (dve.name)
                    {
                        case "BeachGuy":
                        {
                            ConnectManager.Instance.character = Character.Beach;
                            break;
                        }
                        case "AmericanFootballer":
                        {
                            ConnectManager.Instance.character = Character.Football;
                            break;
                        }
                        case "BusinessGuy":
                        {
                            ConnectManager.Instance.character = Character.Business;
                            break;
                        }
                        case "DiscoGuy":
                        {
                            ConnectManager.Instance.character = Character.Disco;
                            break;
                        }
                        case "Farmer":
                        {
                            ConnectManager.Instance.character = Character.Farmer;
                            break;
                        }
                        case "Police":
                        {
                            ConnectManager.Instance.character = Character.Police;
                            break;
                        }
                        case "SoccerGuy":
                        {
                            ConnectManager.Instance.character = Character.Soccer;
                            break;
                        }
                        case "Thief":
                        {
                            ConnectManager.Instance.character = Character.Thief;
                            break;
                        }
                    }
                }
            });
        }
    }
}