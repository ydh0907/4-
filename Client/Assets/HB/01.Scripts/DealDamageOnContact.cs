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
        private int _damage = 10; // 정수형 자료
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody is null) return;

            if (other.attachedRigidbody.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(_damage);
            }
        }
    }
}
