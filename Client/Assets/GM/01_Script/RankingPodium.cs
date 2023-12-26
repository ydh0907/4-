using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPodium : MonoSingleton<RankingPodium>
{
    [SerializeField] Transform[] spawnPositions;

    public void SetPlayerPodium(Transform[] players)
    {
        int i;
        for(i = 0; i< players.Length-1; i++)
        {
            players[i].transform.position = spawnPositions[i].position;
        }
        players[i].transform.position = spawnPositions[i].position;
        players[i].transform.rotation = Quaternion.Euler(90, 0, 90);
    }
}
