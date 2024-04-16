using DH;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class MentosSpawn : NetworkBehaviour
{
    [SerializeField] private Mentos mentosPrefab;
    [SerializeField] private Vector3 offset;
    [SerializeField] private List<Transform> trmIndex = new();
    [SerializeField] private float spawnTerm = 15f;

    private float timer = 0f;
    private Dictionary<Transform, bool> spawnPos = new();

    private void Awake()
    {
        foreach (Transform t in trmIndex)
        {
            spawnPos[t] = true;
        }
    }

    private void Update()
    {
        if (!IsServer) return;
        if (!NetworkGameManager.Instance.IsOnGame.Value) return;

        timer += Time.deltaTime;

        if (timer > spawnTerm)
        {
            Shuffle(trmIndex);

            foreach (Transform trm in trmIndex)
            {
                bool able = spawnPos[trm];

                if (able)
                {
                    SpawnMentos(trm);
                    spawnPos[trm] = false;
                    break;
                }
            }

            timer = 0f;
        }
    }

    private void Shuffle(List<Transform> trmIndex)
    {
        for (int i = 0; i < 100; i++)
        {
            int a = Random.Range(0, trmIndex.Count);
            int b = Random.Range(0, trmIndex.Count);

            var temp = trmIndex[a];
            trmIndex[a] = trmIndex[b];
            trmIndex[b] = temp;
        }
    }

    private void SpawnMentos(Transform trm)
    {
        Vector3 spawnPos = trm.position + offset;

        Mentos mentos = Instantiate(mentosPrefab, spawnPos, Quaternion.identity);
        mentos.GetComponent<NetworkObject>().Spawn();
        mentos.Init(this);

        Debug.Log("Spawn Mentos");
    }

    public void SetPositionState(Transform trm)
    {
        spawnPos[trm] = true;
    }
}
