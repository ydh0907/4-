using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private List<Transform> spawnPoints;

        public int count => spawnPoints.Count;

        public Vector3 GetSpawnPosition()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        }

        public Vector3 GetSpawnPosition(int rand)
        {
            return spawnPoints[rand].position;
        }
    }
}
