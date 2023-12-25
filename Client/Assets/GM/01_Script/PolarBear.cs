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
        [SerializeField] private float playerDeathTime;
        [SerializeField] private int attackDamageAmount;

        private PlayerDamageble playerDamageble;

        private void Start()
        {
            Destroy(gameObject, 2f);
            StartCoroutine("PlayerDeathCoroutine");
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        public void SetPlayerDamageble(Transform playerTrm)
        {
            playerDamageble = playerTrm.GetComponent<PlayerDamageble>();
        }

        private IEnumerator PlayerDeathCoroutine()
        {
            yield return new WaitForSeconds(playerDeathTime);
            playerDamageble.Die();
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
