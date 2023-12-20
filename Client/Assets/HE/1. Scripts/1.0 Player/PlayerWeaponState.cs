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
        if (CanChangeToMentosState())
        {
            _attackDamageAmount = mentosDamageAmount;
        }
        else
            //_attackDamageAmount = �ָ� �Ĺ̴�;
            Debug.Log("dfs");
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
        Vector2 hitDirection = other.gameObject.transform.position - transform.position;

        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("DRINK"))
        {
            iDamageble.Damage(_attackDamageAmount, hitDirection);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            hitDirection.Normalize();
            PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x); // ���� �ϴ��� Ȯ�� �ǤӤ���
        }
    }

    // �Ĺ����� �� �� �ڽó� �Բ� ���� ���� ����� ���� ����� ���� di �Լ� ������ �� ���������� ���� ������� ���� �ο�
    // ���佺 ���¸� �Ĺ����� �ٲ� �ڤ� ����
}
