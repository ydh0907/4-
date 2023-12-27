using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HB
{
    public class PlayerAttack : NetworkBehaviour
    {
        #region COMPONENTS
        private Animator Animator;
        #endregion

        private PlayerDamageble PlayerDamageble;
        private PlayerWeaponState PlayerWeaponState;

        #region STATE PARAMETERS
        public int CurrentMentosCount { get; set; }

        private bool _attackRefilling;

        [SerializeField] private float _attackRefillTime;
        [SerializeField] private float _attackSpeed;
        #endregion

        private void Awake()
        {
            PlayerDamageble = GetComponent<PlayerDamageble>();
        }

        private void Start()
        {
            CurrentMentosCount = 0;
        }

        [ClientRpc]
        public void AttackAniClientRpc()
        {
            Animator.SetTrigger("isAttacking");
        }

        private void Update()
        {
            if (!PlayerWeaponState || !Animator)
            {
                Animator = GetComponentInChildren<Animator>();
                PlayerWeaponState = GetComponentInChildren<PlayerWeaponState>();

                return;
            }

            if (!IsOwner) return;

            #region INPUT HANDLER
            // attack
            if (CanAttack() && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
            #endregion

            Animator.SetFloat("AttackSpeed", _attackSpeed);

            PlayerWeaponState.isMentosAvailable = CurrentMentosCount > 0;
            if (CurrentMentosCount <= 0)
            {
                CurrentMentosCount = 0;
                PlayerWeaponState.isMentosAvailable = false;
            }
        }

        private void Attack()
        {
            if (PlayerWeaponState.IsInMentosState)
                CurrentMentosCount--;

            StartCoroutine(nameof(RefillAttack));
            Animator.SetTrigger("isAttacking");
            AttackAniClientRpc();
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

        private void OnTriggerEnter(Collider other)
        {
            Vector2 hitDirection = other.gameObject.transform.position - transform.position;

            if (other.gameObject.layer == LayerMask.NameToLayer("MENTOS"))
            {
                CurrentMentosCount++;
            }
        }
    }
}