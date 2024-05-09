using DH;
using Packets;
using System.Collections.Generic;
using TestClient;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH
{
    public class CreateRoom : UI
    {
        [SerializeField] protected VisualTreeAsset createRoomTemplate;
        [SerializeField] protected VisualTreeAsset roomListBox;

        protected List<Button> roomList = new List<Button>();

        private VisualElement createRoomList;
        private Room SelectedRoom;
        private int createRoomCount = 0;

        private DragScrollView _dragView;

        protected override void Awake() {
            base.Awake();
            _dragView = GetComponent<DragScrollView>();
        }

        private void CreateRoomList(List<Room> room)
        {
            var template = InstantiateTemplate(createRoomTemplate, root, "container");

            _dragView.SettingScrollView("roomList", true);
            root.Q<Button>("refresh-btn").RegisterCallback<ClickEvent>(HandleRefresh);
            root.Q<Button>("enterRoom-btn").RegisterCallback<ClickEvent>(HandleEnterRoom);
            root.Q<Button>("back-btn").RegisterCallback<ClickEvent>(HandleBackSceneButton);

            int index = 0;
            createRoomList = template.Q<VisualElement>("unity-content-container");
            for (int i = 0; i < createRoomCount; i++)
            { // ������ roomBox�� ���� �� ����
                var roomBoxTemplate = roomListBox.Instantiate().Q<VisualElement>("roomListBox");

                var nickname = roomBoxTemplate.Q<Label>("ninkname-txt");
                var peopleCount = roomBoxTemplate.Q<Label>("peopleCount");

                nickname.text = room[index].makerName;
                peopleCount.text = room[index].playerCount.ToString();

                createRoomList.Add(roomBoxTemplate);

                ++index;
            }
        }
        private void ChooseRoom(List<Room> room)
        {
            for (int i = 0; i < createRoomList.childCount; i++)
            {
                if (createRoomList[i] as Button != null)
                {
                    roomList.Add(createRoomList[i] as Button);
                }
            }

            createRoomList.RegisterCallback<ClickEvent>(evt =>
            {
                var dve = evt.target as Button;
                if (dve != null)
                {
                    OnChooseRoom(roomList, dve);

                    SelectedRoom = room[roomList.IndexOf(dve)];
                }
            });
        }
        private void Refresh(List<Room> room)
        {
            createRoomCount = room.Count;
            roomList.Clear();
            CreateRoomList(room);
            ChooseRoom(room);
            CurrentCharacterDataUI.instance.gameObject.SetActive(false);
        }
        private void OnChooseRoom(List<Button> list, Button dve)
        {
            foreach (var button in list)
            {
                button.RemoveFromClassList("choose");
                if (button == dve)
                {
                    button.AddToClassList("choose");
                }
            }
        }

        public void HandleRefresh(ClickEvent evt)
        {
            _lobbyMain.ButtonClick();
            SelectedRoom = null;
            Program.Instance.Reload(Refresh);
        }
        private void HandleEnterRoom(ClickEvent evt)
        {
            _lobbyMain.ButtonClick();
            ConnectManager.Instance.StartClient(SelectedRoom.roomName, _lobbyMain.GetNickName());
        }
        private void HandleBackSceneButton(ClickEvent evt)
        {
            LoadSceneManager.Instance.LoadScene(2);
        }
    }
}
