using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HB
{
    public class DealDamageOnContact : NetworkBehaviour
    {
        [SerializeField]
        private int _damage; // 정수형 자료

        private bool isOnAttack; // 어택중인가

        private PlayerWeaponState _weaponState;

        private void Start()
        {
            _weaponState = GetComponent<PlayerWeaponState>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody is null) return;

            if (other.attachedRigidbody.TryGetComponent<Health>(out Health health))
            {
                if (!Attack.Instance.isAttack) return;

                if (_weaponState.IsInMentosState)
                {
                    _damage = _weaponState.mentosDamageAmount;
                }

                else
                    _damage = _weaponState.fistDamageAmount;

                health.TakeDamage(_damage);
            }
        }
    }
}
