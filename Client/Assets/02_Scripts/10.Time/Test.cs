using AH;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour {
    [SerializeField] private TimeCounter _counter;
    public VisualElement root;
    private UIDocument _uiDocument;

    private void Awake() {
        _uiDocument = GetComponent<UIDocument>();
    }
    private void OnEnable() {
        root = _uiDocument.rootVisualElement;

        ResurrectionCounter();
    }
    public void ResurrectionCounter(Action callback = null) { // 부활 카운트 다운
        var countText = root.Q<VisualElement>("resurrection-container").Q<Label>("die-txt");

        _counter.ResurrectionCountDown(countText);
    }
}
