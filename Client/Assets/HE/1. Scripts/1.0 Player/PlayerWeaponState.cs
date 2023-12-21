using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponState : MonoBehaviour
{
    private PlayerMovement PlayerMovement;
    private PlayerKnockback PlayerKnockback;
    

    [SerializeField] private bool isMentosAvailable; // 임시로 인스펙터 창에 표시함

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

        Vector2 hitDirection = other.gameObject.transform.position - transform.position;

        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("DRINK"))
        {
            iDamageble.Damage(_attackDamageAmount, hitDirection);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            hitDirection.Normalize();
            PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x);

        }
    }

    // ㅔ미지를 불 ㅒ 자시노 함께 ㅇㅇ 맞은 사람이 ㅐ린 사람을 저장 di 함수 실행할 ㅒ 마지막으로 ㅐ린 사람에게 점수 부여
    // 멘토스 상태면 ㅔ미지만 바뀜 코ㅡ 수정
}
