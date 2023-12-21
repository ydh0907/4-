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
            spawnPos = transform.GetChild(0);
        }

        private void Update()
        {
            currentTime += Time.deltaTime;
            if(currentTime > spawnCoolTime)
            {
                GameObject mantosObj = Instantiate(mentosPrefab, spawnPos.position, Quaternion.identity);
                Material mat = mantosObj.GetComponent<MeshRenderer>().material;
                mat.color = Random.ColorHSV();

                currentTime = 0;
            }
        }
    }
}
