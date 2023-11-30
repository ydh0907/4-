using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class Test : MonoBehaviour
    {
        SpawnPolarBear polarBear;
        ColarSystem colarSystem;

        void Awake()
        {
            polarBear = FindObjectOfType<SpawnPolarBear>();
            colarSystem = FindObjectOfType<ColarSystem>();  
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                polarBear.CallPolarBear(transform);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                colarSystem.StartRush();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                colarSystem.StopRush();
            }
        }
    }
}
