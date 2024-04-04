using System;
using System.Collections;
using System.Collections.Generic;
using DH;
using PJH;
using UnityEngine;
using UnityEngine.InputSystem;

public class CurrentCharacterDataUI : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    private Dictionary<Cola, GameObject> _drinkObjects = new();
    private Dictionary<Character, GameObject> _characterObjects = new();

    private GameObject _currentDrinkObject;
    private Cola _currentDrinkType;
    private GameObject _currentCharacterObject;
    private Character _currentCharacterType;

    private void Awake()
    {
        Transform drinkTrm = transform.Find("Drink");
        Transform characterTrm = transform.Find("Character");

        for (int i = 0; i < drinkTrm.childCount; i++)
        {
            GameObject drink = drinkTrm.GetChild(i).gameObject;
            _drinkObjects.Add((Cola)i, drink);
            drink.gameObject.SetActive(false);
        }

        for (int i = 0; i < characterTrm.childCount; i++)
        {
            GameObject character = characterTrm.GetChild(i).gameObject;
            _characterObjects.Add((Character)i, character);
            character.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        ChangeCharacterData(ConnectManager.Instance.cola, ConnectManager.Instance.character);


        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(0f, -Input.GetAxis("Mouse X") * _rotSpeed * Time.deltaTime, 0f, Space.World);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }


    public void ChangeCharacterData(Cola cola, Character character)
    {
        if (cola != _currentDrinkType)
        {
            if (_currentDrinkObject != null)
            {
                _currentDrinkObject.SetActive(false);
            }

            _currentDrinkObject = _drinkObjects[cola];
            _currentDrinkObject.SetActive(true);
            _currentDrinkType = cola;
        }

        if (character != _currentCharacterType)
        {
            if (_currentCharacterObject != null)
            {
                _currentCharacterObject.SetActive(false);
            }

            _currentCharacterObject = _characterObjects[character];
            _currentCharacterObject.SetActive(true);
            _currentCharacterType = character;
        }
    }
}