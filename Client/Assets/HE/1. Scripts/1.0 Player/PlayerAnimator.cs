using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerMovement PlayerMovement;

    private Animator Animator;

    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();

        Animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        // Animator.SetFloat("x", RB.velocity.x);
        // �ִϸ��̼��� �߰��� ����� �� �Ǵ°� Ʈ���� ��)���� ����, ��ü �ִϸ��̼�??
        // �߰��� ���ܵ� �Ǵ°Ÿ� bool ��) ��, ����
    }
}
