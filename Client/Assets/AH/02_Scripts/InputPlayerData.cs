using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH {
    public class InputPlayerData : MonoBehaviour {
        private UIDocument _uiDocument;
        //private TextField _txtNickname; // 닉네임
        [SerializeField] private VisualTreeAsset createRoomTemplate;
        [SerializeField] private VisualTreeAsset roomListBox;

        [SerializeField] private List<Button> drinksList = new List<Button>();
        [SerializeField] private List<Button> playerList = new List<Button>();
        [SerializeField] private List<Button> roomList = new List<Button>();

        VisualElement root;

        private Button lastChoose = null;
        private string drinkData = null;
        private string playerData = null;

        private int createRoomCount = 0;

        VisualElement createRoomList;

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
                    drinkData = dve.name;
                }
            });
            playerButtonRow.RegisterCallback<ClickEvent>(evt => {
                var dve = evt.target as Button;
                if (dve != null) {
                    ClearToButtonList(playerList, dve);
                    playerData = dve.name;
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
            Debug.Log(drinkData);
            Debug.Log(playerData);
            if (IsInData()) {
                SceneManager.LoadScene("Ingame"); // host
            }
            else {
                Debug.Log("값이 비었어요!");
            }
        }
        private void HandleBackTitleScene(ClickEvent evt) { // host
            SceneManager.LoadScene("Title");
        }

        #region find room
        private void HandleFindRoom(ClickEvent evt) {
            if (!IsInData()) { // 데이터가 다 들어 있다면
                return;
            }
            CreateRoomList();
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
                }
            });
        }
        private void CreateRoomList() {
            var template = createRoomTemplate.Instantiate().Q<VisualElement>("container");

            root.Clear();
            root.Add(template);

            root.Q<Button>("refresh-btn").RegisterCallback<ClickEvent>(HandleRefresh);
            root.Q<Button>("enterRoom-btn").RegisterCallback<ClickEvent>(HandleEnterRoom);

            createRoomCount = Random.Range(0, 8);

            createRoomList = template.Q<VisualElement>("unity-content-container");
            for (int i = 0; i < createRoomCount; i++) { // 생성할 roomBox의 개수 및 생성
                var roomBoxTemplate = roomListBox.Instantiate().Q<VisualElement>("roomListBox");

                var nickname = roomBoxTemplate.Q<Label>("ninkname-txt");
                var peopleCount = roomBoxTemplate.Q<Label>("peopleCount");
                // 위에서 받아온 값으로 닉네임와 플레이어 카운트를 조절한다

                createRoomList.Add(roomBoxTemplate);
            }
        }

        private void HandleRefresh(ClickEvent evt) {
            createRoomCount = Random.Range(0, 8);
            CreateRoomList();
        }
        private void HandleEnterRoom(ClickEvent evt) {
            if(lastChoose != null) {
                SceneManager.LoadScene("Ingame");
            }
            else {
                Debug.Log("값이 비었어요!");
            }
        }
        private bool IsInData() {
            return drinkData != null && playerData != null ? true : false;
        }
        private void ClearToRoomList(List<Button> list, Button dve) {
            foreach (var button in list) {
                Debug.Log(button);
                button.RemoveFromClassList("choose");
                if (button == dve) {
                    button.AddToClassList("choose");
                }
            }
        }
        #endregion
    }
}
