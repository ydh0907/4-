using System.Collections.Generic;
using UnityEngine;

namespace GM {
    public class MapManager : MonoSingleton<MapManager> {
        [SerializeField] private List<Transform> spawnPosList;

        public Vector3 GetSpawnPosition() {
            int index = Random.Range(0, spawnPosList.Count);
            Transform pos = spawnPosList[index];
            spawnPosList.Remove(pos);
            Debug.Log($"SpawnPsoListCount : {spawnPosList.Count}");
            return spawnPosList[index].position;
        }
    }
}
