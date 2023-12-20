using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponState : MonoBehaviour
{
    private PlayerMovement PlayerMovement;
    private PlayerKnockback PlayerKnockback;
    

    [SerializeField] private bool isMentosAvailable; // �ӽ÷� �ν����� â�� ǥ����

    private int _attackDamageAmount;
    public int mentosDamageAmount;
    public int fistDamageAmount;

    private void Awake()
    {
        PlayerMovement = GetComponentInParent<PlayerMovement>();
        PlayerKnockback = GetComponentInParent<PlayerKnockback>();
    }


    private void Update()
    {
        if (CanChangeToMentosState())
        {
            SetToMentosState();
        }
    }

    private void SetToMentosState()
    {
        _attackDamageAmount = mentosDamageAmount;
    }

    private bool CanChangeToMentosState()
    {
        if (isMentosAvailable)
        {
            return true;
        }
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageble iDamageble = other.gameObject.GetComponent<IDamageble>();

        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("DRINK"))
        {
            Vector2 hitDirection = other.gameObject.transform.position - transform.position;
            hitDirection.Normalize();
            iDamageble.Damage(_attackDamageAmount, hitDirection);
            PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {

        }
    }

    // �Ĺ����� �� �� �ڽó� �Բ� ���� ���� ����� ���� ����� ���� di �Լ� ������ �� ���������� ���� ������� ���� �ο�
}
