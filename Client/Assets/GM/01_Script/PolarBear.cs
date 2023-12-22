using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class PolarBear : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float playerDeathTime;

        private void Start()
        {
            Destroy(gameObject, 2f);
            StartCoroutine("PlayerDeathCoroutine");
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private IEnumerator PlayerDeathCoroutine()
        {
            yield return new WaitForSeconds(playerDeathTime);
            Debug.Log("플레이어 죽음");
        }
    }
}
