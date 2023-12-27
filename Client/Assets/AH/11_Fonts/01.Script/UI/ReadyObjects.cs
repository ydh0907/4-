using DH;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ReadyObjects : NetworkBehaviour
{
    public static ReadyObjects Instance = null;

    [SerializeField] private GameObject Nickname;

    [SerializeField] private List<Transform> SpawnPos = new();
    public Dictionary<ulong, TextMeshPro> nicknames = new();

    List<GameObject> Dummys = new();

    private bool start = false;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        SetCurrentCharactersClientRpc();
        SetNicknameColorClientRpc();
    }

    [ClientRpc]
    public void SetCurrentCharactersClientRpc()
    {
        if (start) return;

        int index = 0;

        foreach(var d in Dummys)
        {
            Destroy(d);
        }

        Dummys.Clear();
        nicknames.Clear();

        foreach(var player in NetworkGameManager.Instance.players)
        {
            GameObject character = Instantiate(NetworkGameManager.Instance.Characters[(int)player.Value.Char], SpawnPos[index].position, SpawnPos[index].rotation);
            TextMeshPro text = Instantiate(Nickname, character.transform).GetComponent<TextMeshPro>();
            text.text = player.Value.Nickname;
            Instantiate(NetworkGameManager.Instance.Drinks[(int)player.Value.Cola], character.transform).transform.position += new Vector3(0, 0, -0.3f);

            Dummys.Add(character);
            nicknames.Add(player.Key, text);

            index++;
        }
    }

    [ClientRpc]
    public void SetNicknameColorClientRpc()
    {
        foreach(var player in NetworkGameManager.Instance.players)
        {
            nicknames[player.Key].color = player.Value.Ready ? Color.green : Color.red;
        }
    }

    [ClientRpc]
    public void RemoveClientRpc()
    {
        foreach (var d in Dummys)
        {
            Destroy(d);
        }

        Dummys.Clear();

        start = true;
    }
}
