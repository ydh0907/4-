using AH;
using Packets;
using System;
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
        public void SettingScrollView(string scrollViewName, bool isVertical) {
            _scrollView = root.Q<ScrollView>(scrollViewName);

            _content = _scrollView.contentViewport; // ��� content�� ����ִ� �θ�?�� ������

            _content.RegisterCallback<PointerDownEvent>(HandleOnPointerDown); // Pointer�� ������ ��
            _content.RegisterCallback<PointerMoveEvent, bool>(HandleOnPointerMove, isVertical); // Pointer�� �����̰� ���� ��
            _content.RegisterCallback<PointerUpEvent>(HandleOnPointerUp); // Pointer�� �÷��� ��

        }
        private void HandleOnPointerDown(PointerDownEvent evt) {
            _lastMousePos = evt.localPosition;
            evt.StopPropagation();
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
        }
        private void HandleOnPointerUp(PointerUpEvent evt) {
            evt.StopPropagation();
        }
    }
}
