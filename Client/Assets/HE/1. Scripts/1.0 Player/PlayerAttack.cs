using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x);

    public int _attackDamageAmount;
    // ���߿� ���� �ð� � ���� �� ��� �Ұž�..

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
