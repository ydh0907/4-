using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class SpawnPolarBear : MonoBehaviour
    {
        [SerializeField] private GameObject polarBearPrefab;
        [SerializeField] private float spawnDistance;

        public void CallPolarBear(Transform playerTrm)
        {
            Vector3 spawnPos = playerTrm.position + (playerTrm.forward * spawnDistance);
            GameObject polarBearObj = Instantiate(polarBearPrefab, spawnPos, Quaternion.identity);
            polarBearObj.transform.forward = (playerTrm.position - polarBearObj.transform.position).normalized;
        }
    }
}
