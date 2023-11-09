using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class SpawnMentos : MonoBehaviour
    {
        [SerializeField] private GameObject mentosPrefab;
        [SerializeField] private float spawnCoolTime;

        private Transform spawnPos;
        private float currentTime = 0;

        private void Start()
        {
            spawnPos = GetComponentInChildren<Transform>();
        }

        private void Update()
        {
            currentTime += Time.deltaTime;
            if(currentTime > spawnCoolTime)
            {
                Instantiate(mentosPrefab, spawnPos.position, Quaternion.identity);

                currentTime = 0;
            }
        }
    }
}
