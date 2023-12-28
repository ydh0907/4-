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

    [SerializeField] GameObject Nickname;
    [SerializeField] Transform[] spawnPositions;

    [SerializeField] public List<GameObject> Characters => NetworkGameManager.Instance.Characters;
    [SerializeField] public List<GameObject> Drinks => NetworkGameManager.Instance.Drinks;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayerPodium(Dictionary<ulong, PlayerInfo> users)// 1등부터 내림차순 정렬
    {
        List<PlayerInfo> list = new();

        foreach(var user in users)
        {
            list.Add(user.Value);
        }

        PlayerInfo temp;
        for(int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].kill < list[i + 1].kill)
            {
                temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
            }
        }

        for(int i = 0; i < list.Count; i++)
        {
            temp = list[i];

            GameObject character = Instantiate(NetworkGameManager.Instance.Characters[(int)temp.Char], spawnPositions[i].position, spawnPositions[i].rotation);
            TextMeshPro text = Instantiate(Nickname, character.transform).GetComponent<TextMeshPro>();
            text.text = temp.Nickname;
            Instantiate(NetworkGameManager.Instance.Drinks[(int)temp.Cola], character.transform).transform.position += new Vector3(0, 0, -0.3f);
        }
    }
}
