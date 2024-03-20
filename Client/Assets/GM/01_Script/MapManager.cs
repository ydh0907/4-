using System.Collections.Generic;
using UnityEngine;

namespace GM {
    public class MapManager : MonoSingleton<MapManager> {
        [SerializeField] private List<Transform> spawnPosList;

        public int count => spawnPosList.Count;

        public Vector3 GetSpawnPosition() {
            Debug.Log(count);
            int index = Random.Range(0, spawnPosList.Count);
            Debug.Log(index);
            Transform pos = spawnPosList[index];
            spawnPosList.Remove(pos);
            Debug.Log($"SpawnPsoListCount : {spawnPosList.Count}");
            return spawnPosList[index].position;
        }
    }
}
