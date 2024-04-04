using System;
using System.Collections;
using System.Collections.Generic;
using DH;
using PJH;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CurrentCharacterDataUI : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    private Dictionary<Cola, GameObject> _drinkObjects = new();
    private Dictionary<Character, Animator> _characterObjects = new();

    private GameObject _currentDrinkObject;
    private Cola _currentDrinkType;
    private Animator _currentCharacterObject;
    private Character _currentCharacterType;

    private int _prevIdleIdx;

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
            Animator character = characterTrm.GetChild(i).gameObject.GetComponent<Animator>();
            _characterObjects.Add((Character)i, character);
            character.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(RandomIdle());
        ChangeCharacterData(ConnectManager.Instance.cola, ConnectManager.Instance.character, true);
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
            transform.Rotate(0f, -Input.GetAxis("Mouse X") * _rotSpeed, 0f, Space.World);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private IEnumerator RandomIdle()
    {
        yield return new WaitUntil(() => _currentCharacterObject != null);
        while (true)
        {
            yield return new WaitUntil(() => _currentCharacterObject.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            int idleIdx = Random.Range(1, 3);
            while (idleIdx == _prevIdleIdx)
            {
                idleIdx = Random.Range(1, 3);
                yield return null;
            }

            _prevIdleIdx = idleIdx;
            _currentCharacterObject.CrossFadeInFixedTime($"Idle{idleIdx}", .4f);
            yield return null;
            yield return new WaitForSeconds(2f);
        }
    }


    public void ChangeCharacterData(Cola cola, Character character, bool immediate = false)
    {
        if (cola != _currentDrinkType || immediate)
        {
            if (_currentDrinkObject != null)
            {
                _currentDrinkObject.gameObject.SetActive(false);
            }

            _currentDrinkObject = _drinkObjects[cola];
            _currentDrinkObject.gameObject.SetActive(true);
            _currentDrinkType = cola;
        }

        if (character != _currentCharacterType || immediate)
        {
            if (_currentCharacterObject != null)
            {
                _currentCharacterObject.Play("Idle");
                _currentCharacterObject.gameObject.SetActive(false);
            }

            _currentCharacterObject = _characterObjects[character];
            _currentCharacterObject.gameObject.SetActive(true);
            _currentCharacterType = character;
        }
    }
}