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

    public void SetCurrentCharacters()
    {
        if (start) return;

        int index = 0;

        foreach(var d in Dummys)
        {
            Destroy(d);
        }

        Dummys.Clear();

        foreach(var player in NetworkGameManager.Instance.players)
        {
            GameObject character = Instantiate(NetworkGameManager.Instance.Characters[(int)player.Value.Char], SpawnPos[index].position, SpawnPos[index].rotation);
            TextMeshPro text = Instantiate(Nickname, character.transform).GetComponent<TextMeshPro>();
            text.text = player.Value.Nickname;
            Instantiate(NetworkGameManager.Instance.Drinks[(int)player.Value.Cola], character.transform).transform.position += new Vector3(0, 0, -0.3f);

            Dummys.Add(character);
            nicknames[player.Key] = text;

            index++;
        }
    }

    [ClientRpc]
    public void SetNicknameColorClientRpc(ulong id, Color color)
    {
        nicknames[id].color = color;
    }

    public void Remove()
    {
        foreach (var d in Dummys)
        {
            Destroy(d);
        }

        Dummys.Clear();

        start = true;
    }
}
