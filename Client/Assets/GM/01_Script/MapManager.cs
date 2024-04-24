using System.Collections.Generic;
using UnityEngine;

namespace GM {
    public class MapManager : MonoSingleton<MapManager> {
        [SerializeField] private List<Transform> spawnPosList;

        public List<Vector3> GetSpawnList()
        {
            List<Vector3> spawnPos = new();
            spawnPosList.ForEach(pos => spawnPos.Add(pos.position));
            return spawnPos;
        }

        public Vector3 GetSpawnPosition() {
            int index = Random.Range(0, spawnPosList.Count);
            Transform pos = spawnPosList[index];
            return spawnPosList[index].position;
        }
    }
}
