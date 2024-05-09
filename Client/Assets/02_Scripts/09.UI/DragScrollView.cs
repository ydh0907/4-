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
        /// <param name="scrollViewName">������ scrollView�� �̸�</param>
        /// <param name="isVertical">t : ����, f : ����</param>
        public void SettingScrollView(string scrollViewName, bool isVertical, List<VisualElement> roomList) {
            _scrollView = root.Q<ScrollView>(scrollViewName);

            _content = _scrollView.contentViewport; // ��� content�� ����ִ� �θ�?�� ������

            _content.RegisterCallback<PointerDownEvent, List<VisualElement>>(HandleOnPointerDown, roomList); // Pointer�� ������ ��
            _content.RegisterCallback<PointerMoveEvent, bool>(HandleOnPointerMove, isVertical); // Pointer�� �����̰� ���� ��
            _content.RegisterCallback<PointerUpEvent>(HandleOnPointerUp); // Pointer�� �÷��� ��

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
            if (evt.isPrimary && evt.pressedButtons == 1) { // �⺻ �������̳�? ������ �ֳ�?
                Vector2 delta = _lastMousePos - evt.localPosition;

                // isVertical�� ���� ���� Ư�� ������ �̵� �� �� ���� ����
                Vector2 pos = isVertical == true ? new Vector2(0, delta.y) : new Vector2(delta.x, 0);

                _scrollView.scrollOffset += pos;

                _lastMousePos = evt.localPosition; // ������ ��ġ ������Ʈ

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
