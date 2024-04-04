using DH;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;


namespace AH {
    public class LobbyMain : UI {
        private CreateRoom _createRoom;
        public VisualElement _dataBorder;
        private CustomPlayerAndDrinkData _playerData;

        public string nickname = "�г���";
        bool flag;
        [SerializeField] private List<Sprite> _characterSprites, _drinkSprites;

        protected override void Awake() {
            base.Awake();
            _createRoom = GetComponent<CreateRoom>();
            _playerData = GetComponent<CustomPlayerAndDrinkData>();
        }
        protected override void OnEnable() {
            base.OnEnable();
            _dataBorder = root.Q<VisualElement>("container");
            flag = true;

            root.Q<Button>("backToTitle-btn").RegisterCallback<ClickEvent>(HandleBackTitleScene);
            root.Q<Button>("create-btn").RegisterCallback<ClickEvent>(HandleCreateRoom);
            root.Q<Button>("findroom-btn").RegisterCallback<ClickEvent>(HandleFindRoom);
            CustomDataSetting();
        }

        public void CustomDataSetting() {
            var nickNameField = root.Q<TextField>("nickname-inputfeld");
            nickNameField.value = nickname;
            nickNameField.RegisterCallback<ChangeEvent<string>>(OnNicknameChanged);
            root.Q<Button>("customizing-player-btn").RegisterCallback<ClickEvent>((e) => _playerData.HandleGoToChoosePlayer(e, _dataBorder));
            root.Q<Button>("customizing-drink-btn").RegisterCallback<ClickEvent>((e) => _playerData.HandleGoToChooseDrink(e, _dataBorder));
            
        }

        private void OnNicknameChanged(ChangeEvent<string> evt) {
            nickname = evt.newValue.ToString();
        }

        #region Handle
        private void HandleCreateRoom(ClickEvent evt) {
            ButtonClick();
            if(flag)
                ConnectManager.Instance.StartHost(GetNickName());
            flag = false;
        }
        private void HandleFindRoom(ClickEvent evt) {
            ButtonClick();
            GetNickName();
            _createRoom.HandleRefresh(evt);
        }
        private void HandleBackTitleScene(ClickEvent evt) { // host
            if (NetworkManager.Singleton != null) {
                Destroy(NetworkManager.Singleton.gameObject);
            }
            ButtonClick();
            SceneManager.LoadScene(0);
        }
        #endregion

        #region GetData
        public void GetInputListData(VisualElement buttonRow, List<Button> list) {
            for (int i = 0; i < buttonRow.childCount; i++) {
                if (buttonRow[i] as Button != null) {
                    list.Add(buttonRow[i] as Button);
                }
            }
        }
        public string GetNickName() {
            if (root.Q<TextField>("nickname-inputfeld") != null) {
                nickname = root.Q<TextField>("nickname-inputfeld").text;
                Debug.Log(nickname);
            }
            return nickname;
        }
        #endregion

        public void ClearToButtonList(List<Button> list, Button dve) {
            foreach (var button in list) {
                button.RemoveFromClassList("notChoose");
                if (button != dve) {
                    button.AddToClassList("notChoose");
                }
            }
        }
        public void ButtonClick() {
            SoundManager.Instance.Play("Effect/Button click");
        }
    }
}
