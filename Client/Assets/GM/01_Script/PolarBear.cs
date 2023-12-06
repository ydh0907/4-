using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class PolarBear : MonoBehaviour
    {
        [SerializeField] private float speed;

        private void Start()
        {
            Destroy(gameObject, 20f);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
