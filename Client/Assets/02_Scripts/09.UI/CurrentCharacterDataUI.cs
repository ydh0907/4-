using System;
using System.Collections;
using System.Collections.Generic;
using DH;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CurrentCharacterDataUI : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    [SerializeField] private LayerMask _whatIsCharacter;
    [SerializeField] private Transform[] _bodies;
    private Dictionary<Cola, GameObject> _drinkObjects = new();
    private Dictionary<Character, Animator> _characterObjects = new();

    private GameObject _currentDrinkObject;
    private Cola _currentDrinkType;
    private Animator _currentCharacterObject;
    private Character _currentCharacterType;

    private int _prevIdleIdx;


    public static CurrentCharacterDataUI instance;

    private Vector2 _beforeHideMousePos;


    private void Awake()
    {
        instance = this;
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
        if (Cursor.visible)
            _beforeHideMousePos = Mouse.current.position.value;
        if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (IsMousePointerOnCharacter())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
        {
            if (!Cursor.visible)
                transform.Rotate(0f, -Input.GetAxis("Mouse X") * _rotSpeed, 0f, Space.World);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (Cursor.visible) return;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Mouse.current.WarpCursorPosition(_beforeHideMousePos);
            // Cursor.lockState = CursorLockMode.None;
        }
    }

    private bool IsMousePointerOnCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, 20f, _whatIsCharacter);
    }

    private IEnumerator RandomIdle()
    {
        yield return new WaitUntil(() => _currentCharacterObject != null);
        while (true)
        {
            yield return new WaitUntil(() => _currentCharacterObject.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            int idleIdx = Random.Range(1, 5);
            while (idleIdx == _prevIdleIdx)
            {
                idleIdx = Random.Range(1, 5);
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
            _currentDrinkObject.transform.SetParent(_bodies[(int)character]);
            _currentDrinkObject.transform.localPosition = new Vector3(0, -0.002089481f, -0.004531645f);


            _currentDrinkObject.transform.localRotation = Quaternion.identity;
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
            _currentDrinkObject.transform.SetParent(_bodies[(int)character]);
            _currentDrinkObject.transform.localPosition = new Vector3(0, -0.002089481f, -0.004531645f);
            _currentDrinkObject.transform.localRotation = Quaternion.identity;

            _currentCharacterType = character;
        }
    }
}