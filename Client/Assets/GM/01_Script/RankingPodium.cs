using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPodium : MonoBehaviour
{
    private static RankingPodium instance;
    public static RankingPodium Instance { get { return instance; } }

    [SerializeField] Transform[] spawnPositions;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayerPodium(Transform[] players)// 1등부터 내림차순 정렬
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
