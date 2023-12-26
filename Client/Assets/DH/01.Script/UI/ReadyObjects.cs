using DH;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReadyObjects : NetworkBehaviour
{
    [SerializeField] private List<Transform> SpawnPos = new();

    public void SetCurrentCharacters()
    {
        int index = 0;

        foreach(var player in NetworkGameManager.Instance.players)
        {
            Vector3 pos = SpawnPos[index].position;



            index++;
        }
    }
}
