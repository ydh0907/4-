using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HB;

namespace GM
{
    public class PolarBear : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private int attackDamageAmount;

        private void Start()
        {
            Destroy(gameObject, 2f);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageble iDamageble = other.gameObject.GetComponent<IDamageble>();
            Vector2 hitDirection = other.gameObject.transform.position - transform.position;

            if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("DRINK"))
            {
                iDamageble.Damage(attackDamageAmount, hitDirection);
            }
        }
    }
}
