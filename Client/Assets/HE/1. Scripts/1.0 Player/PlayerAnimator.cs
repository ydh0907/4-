using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HE
{
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
            // 애니메이션이 중간에 끊기면 안 되는건 트리거 예)공격 점프, 교체 애니메이션??
            // 중간에 끊겨도 되는거면 bool 예) 런, 러쉬
        }
    }
}
