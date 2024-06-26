using Unity.Netcode;
using UnityEngine;

public class RankingPodium : NetworkBehaviour {
    private static RankingPodium instance;
    public static RankingPodium Instance { get { return instance; } }

    [SerializeField] Transform[] spawnPositions;

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        instance = this;
    }

    public Transform[] GetPositions() {
        return spawnPositions;
    }
}
