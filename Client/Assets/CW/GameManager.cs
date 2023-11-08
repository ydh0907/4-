using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Karin.PoolingSystem;


namespace Karin
{
    public class GameManager : MonoBehaviour
    {
        public PoolManager PoolManager;

        private void Awake()
        {
            PoolManager = GetComponent<PoolManager>();
        }
    }
}
