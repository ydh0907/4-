using DH;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class RankingPodium : NetworkBehaviour
{
    private static RankingPodium instance;
    public static RankingPodium Instance { get { return instance; } }

    [SerializeField] Transform[] spawnPositions;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        instance = this;

        Debug.Log("Spawned");
    }

    public Transform[] GetPositions()
    {
        return spawnPositions;
    }
}
