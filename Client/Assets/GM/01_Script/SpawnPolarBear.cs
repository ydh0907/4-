using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class SpawnPolarBear : MonoBehaviour
    {
        private static SpawnPolarBear instance = null;

        public static SpawnPolarBear Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SpawnPolarBear>();
                    if (instance == null)
                    {
                        instance = new GameObject("SpawnPolarBear").AddComponent<SpawnPolarBear>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private GameObject polarBearPrefab;
        [SerializeField] private float spawnDistance;

        public void CallPolarBear(Transform playerTrm)
        {
            StartCoroutine(SpawnPolarBearAction(playerTrm));
        }

        IEnumerator SpawnPolarBearAction(Transform playerTrm)
        {
            Vector3 spawnPos = playerTrm.position + (playerTrm.forward * spawnDistance);
            GameObject polarBearObj = Instantiate(polarBearPrefab, spawnPos, Quaternion.identity);
            polarBearObj.transform.forward = (playerTrm.position - polarBearObj.transform.position).normalized;

            yield return null; 
        }
    }
}
