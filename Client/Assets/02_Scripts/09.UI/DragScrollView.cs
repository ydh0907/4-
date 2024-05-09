using AH;
using Packets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class DragScrollView : UI {
        private ScrollView _scrollView;
        private VisualElement _content;

        private Vector3 _lastMousePos;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scrollViewName">가져올 scrollView의 이름</param>
        /// <param name="isVertical">t : 세로, f : 가로</param>
        public void SettingScrollView(string scrollViewName, bool isVertical, List<VisualElement> roomList) {
            _scrollView = root.Q<ScrollView>(scrollViewName);

            _content = _scrollView.contentViewport; // 모든 content가 들어있는 부모?를 가져옴

            _content.RegisterCallback<PointerDownEvent, List<VisualElement>>(HandleOnPointerDown, roomList); // Pointer를 내렸을 때
            _content.RegisterCallback<PointerMoveEvent, bool>(HandleOnPointerMove, isVertical); // Pointer가 움직이고 있을 때
            _content.RegisterCallback<PointerUpEvent>(HandleOnPointerUp); // Pointer를 올렸을 때

        }
        private void HandleOnPointerDown(PointerDownEvent evt, List<VisualElement> roomList) {
            foreach(var room in roomList) {
                Debug.Log(room);
                if(evt.currentTarget == room || evt.currentTarget == _content) {
                    _lastMousePos = evt.localPosition;
                    evt.StopPropagation();
                    Debug.Log("down");
                }
            }
        }
        private void HandleOnPointerMove(PointerMoveEvent evt, bool isVertical) {
            if (evt.isPrimary && evt.pressedButtons == 1) { // 기본 보인터이냐? 누르고 있냐?
                Vector2 delta = _lastMousePos - evt.localPosition;

                // isVertical의 값에 따라 특정 축으로 이동 할 수 없게 만듬
                Vector2 pos = isVertical == true ? new Vector2(0, delta.y) : new Vector2(delta.x, 0);

                _scrollView.scrollOffset += pos;

                _lastMousePos = evt.localPosition; // 마지막 위치 업데이트

                evt.StopPropagation();
            }
            Debug.Log("drag");
        }
        private void HandleOnPointerUp(PointerUpEvent evt) {
            evt.StopPropagation();
            Debug.Log("up");
        }
    }
}
