using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPodium : MonoSingleton<RankingPodium>
{
    [SerializeField] Transform[] spawnPositions;

    public void SetPlayerPodium(Transform[] players)
    {
        for(int i = 0; i< players.Length; i++)
        {
            players[i].transform.position = spawnPositions[i].position;
        }
    }
}
