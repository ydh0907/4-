using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        private void Awake()
        {
            if(instance != null) Destroy(instance);
            instance = this;
        }

        [SerializeField] private List<Transform> spawnPoints;

        public Vector3 GetSpawnPosition()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        }
    }
}
