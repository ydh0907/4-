using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x);

    public int _attackDamageAmount;
    // 나중에 공격 시간 등도 설정 해 줘야 할거야..

    private Animator Animator;

    private PlayerDamageble PlayerDamageble;

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // attack
        if (CanAttack() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsAttacking = true;

            Attack();
        }
    }

    private void Attack()
    {
        Animator.SetTrigger("");
    }

    private bool CanAttack()
    {
        if (!PlayerDamageble.IsGroggying)
            return true;
        else
            return false;
    }
}
