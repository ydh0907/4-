using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class PlayerAttack : MonoBehaviour
    {
        #region COMPONENTS
        private Animator Animator;
        #endregion

        private PlayerDamageble PlayerDamageble;

        #region STATE PARAMETERS
        private bool _attackRefilling;

        [SerializeField] private float _attackRefillTime;
        [SerializeField] private float _attackSpeed;
        #endregion

        private void Awake()
        {
            Animator = GetComponent<Animator>();

            PlayerDamageble = GetComponent<PlayerDamageble>();
        }

        private void Update()
        {
            #region INPUT HANDLER
            // attack
            if (CanAttack() && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
            #endregion

            Animator.SetFloat("AttackSpeed", _attackSpeed);
        }

        private void Attack()
        {
            StartCoroutine(nameof(RefillAttack));
            Animator.SetTrigger("isAttacking");
        }

        public IEnumerator RefillAttack()
        {
            _attackRefilling = true;
            yield return new WaitForSeconds(_attackRefillTime);
            _attackRefilling = false;
        }

        private bool CanAttack()
        {
            if (!PlayerDamageble.IsGroggying && !_attackRefilling)
                return true;
            else
                return false;
        }
    }
}

// 애러 발생함 왜? PlayrDamagbl에서 네임 스페이스가 ㅏㄹ라서 컴포넌트를 찾을 수 없음