using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class DealDamageOnContact : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 10;

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
