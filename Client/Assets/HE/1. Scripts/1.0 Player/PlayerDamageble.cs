using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageble : MonoBehaviour, IDamageble
{
    public bool IsGroggying { get; private set; }

    private PlayerMovement PlayerMovement;

    public int CurrentHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    [SerializeField] private float _faintTime;

    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(nameof(GroggyAction));
        }
    }

    public void Damage(int damageAmount, Vector3 hitDirection)
    {
        // StartCoroutine(nameof(FaintAction));
    }

    public void Die() { } // 사용 안 함

    IEnumerator GroggyAction()
    {
        IsGroggying = true;
        yield return new WaitForSeconds(_faintTime);
        IsGroggying = false;
    }
}
