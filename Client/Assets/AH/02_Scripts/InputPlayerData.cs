using DH;
using Packets;
using System;
using System.Collections.Generic;
using TestClient;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH {
    public class InputPlayerData : MonoBehaviour {
        private UIDocument _uiDocument;

        [SerializeField] private VisualTreeAsset createRoomTemplate;
        [SerializeField] private VisualTreeAsset roomListBox;

        [SerializeField] private List<Button> drinksList = new List<Button>();
        [SerializeField] private List<Button> playerList = new List<Button>();
        [SerializeField] private List<Button> roomList = new List<Button>();

        VisualElement root;

        string nickname = "";
        private Button lastChoose = null;
        Room SelectedRoom;

        private int createRoomCount = 0;

        VisualElement createRoomList;

        private void Awake() {
            _uiDocument = GetComponent<UIDocument>();
        }
        private void OnEnable() {
            root = _uiDocument.rootVisualElement;


            root.Q<Button>("backToTitle-btn").RegisterCallback<ClickEvent>(HandleBackTitleScene);
            root.Q<Button>("create-btn").RegisterCallback<ClickEvent>(HandleCreateRoom);
            root.Q<Button>("findroom-btn").RegisterCallback<ClickEvent>(HandleFindRoom);

            #region list값 넣기
            VisualElement drinkButtonRow = root.Q<VisualElement>(className: "drink-content");
            for (int i = 0; i < drinkButtonRow.childCount; i++) {
                if (drinkButtonRow[i] as Button != null) {
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
            playerButtonRow.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    ClearToButtonList(playerList, dve);
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

        private string GetNickName() {
            if (root.Q<TextField>("nickname-inputfeld") != null) nickname = root.Q<TextField>("nickname-inputfeld").text;
            return nickname;
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
            ConnectManager.Instance.StartHost(GetNickName());
        }
        private void HandleBackTitleScene(ClickEvent evt) { // host
            if (NetworkManager.Singleton != null) Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene(0);
        }

        #region find room
        private void HandleFindRoom(ClickEvent evt) {
            GetNickName();
            HandleRefresh(evt);
        }

        private void CreateRoomList(List<Room> room) {
            var template = createRoomTemplate.Instantiate().Q<VisualElement>("container");

            root.Clear();
            root.Add(template);

            root.Q<Button>("refresh-btn").RegisterCallback<ClickEvent>(HandleRefresh);
            root.Q<Button>("enterRoom-btn").RegisterCallback<ClickEvent>(HandleEnterRoom);

            int index = 0;
            createRoomList = template.Q<VisualElement>("unity-content-container");
            for (int i = 0; i < createRoomCount; i++) { // 생성할 roomBox의 개수 및 생성
                var roomBoxTemplate = roomListBox.Instantiate().Q<VisualElement>("roomListBox");

                var nickname = roomBoxTemplate.Q<Label>("ninkname-txt");
                var peopleCount = roomBoxTemplate.Q<Label>("peopleCount");

                nickname.text = room[index].makerName;
                peopleCount.text = room[index].playerCount.ToString();

                createRoomList.Add(roomBoxTemplate);

                ++index;
            }
        }
        private void ChooseRoom(List<Room> room) {
            for (int i = 0; i < createRoomList.childCount; i++) {
                if (createRoomList[i] as Button != null) {
                    roomList.Add(createRoomList[i] as Button);
                }
            }

            createRoomList.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    ClearToRoomList(roomList, dve);
                    lastChoose = dve;

                    SelectedRoom = room[roomList.IndexOf(dve)];
                }
            });
        }

        private void HandleRefresh(ClickEvent evt) {
            Program.Instance.Reload(Refresh);
        }

        private void Refresh(List<Room> room) {
            createRoomCount = room.Count;
            CreateRoomList(room);
            ChooseRoom(room);
        }

        private void HandleEnterRoom(ClickEvent evt) {
            ConnectManager.Instance.StartClient(SelectedRoom.roomName, GetNickName());
        }

        private void ClearToRoomList(List<Button> list, Button dve) {
            foreach (var button in list) {
                button.RemoveFromClassList("choose");
                if (button == dve) {
                    button.AddToClassList("choose");
                }
            }
        }
        #endregion
    }
}
