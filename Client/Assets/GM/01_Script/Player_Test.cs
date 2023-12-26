using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class Player_Test : MonoBehaviour
    {
        [SerializeField] Transform[] trm;
        SpawnPolarBear polarBear;

        private void Awake()
        {
            polarBear = FindObjectOfType<SpawnPolarBear>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RankingPodium.Instance.SetPlayerPodium(trm);
            }
        }
    }
}
