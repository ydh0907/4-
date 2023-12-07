using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageble : MonoBehaviour, IDamageble
{

    public bool IsFainting { get; private set; } //


    private PlayerMovement PlayerMovement;
    private PlayerKnockback PlayerKnockback;

    public int CurrentHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    [SerializeField] private float _faintTime;
    [SerializeField] private float _resurrectionTime;

    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerKnockback = GetComponent<PlayerKnockback>();    
    }

    public void Damage(int damageAmount, Vector3 hitDirection)
    {
        StartCoroutine(nameof(Faint));
        PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x);
    }

    public void Die()
    {
        Debug.Log("die");
        StartCoroutine(nameof(Resurrection));
    }

    IEnumerator Resurrection()
    {
        yield return new WaitForSeconds(_resurrectionTime);
        Debug.Log("∫Œ»∞");
    }

    IEnumerator Faint()
    {
        IsFainting = true;
        yield return new WaitForSeconds(_faintTime);
        IsFainting = false;
    }
}
