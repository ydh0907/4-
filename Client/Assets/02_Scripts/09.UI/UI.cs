using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class UI : MonoBehaviour {
        protected UIDocument _uiDocument;

        protected VisualElement root;
        protected LobbyMain _lobbyMain;

        protected virtual void Awake() {
            _uiDocument = GetComponent<UIDocument>();
            _lobbyMain = GetComponent<LobbyMain>();
        }
        protected virtual void OnEnable() {
            root = _uiDocument.rootVisualElement;
        }
        protected VisualElement InstantiateTemplate(VisualTreeAsset treeAsset, VisualElement addRoot, string getName, bool isClear = true) {
            var template = treeAsset.Instantiate().Q<VisualElement>(getName);
            if (isClear) {
                addRoot.Clear();
            }
            addRoot.Add(template);
            return template;
        }
    }
}
