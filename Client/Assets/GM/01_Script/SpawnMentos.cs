using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace GM
{
    public class SpawnMentos : NetworkBehaviour
    {
        [SerializeField] private GameObject mentosPrefab;
        [SerializeField] private Transform SpawnTrm;

        private Mentos current = null;

        public static SpawnMentos Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void StartSpawn()
        {
            Debug.Log("StartSpawn");
            StartCoroutine("MentosSpawnCorutine");
        }

        public void StopSpawn()
        {
            Debug.Log("StopSpawn");
            StopCoroutine("MentosSpawnCorutine");
        }

        private IEnumerator MentosSpawnCorutine()
        {
            while (true)
            {
                float randNum = Random.Range(10, 20);
                yield return new WaitForSeconds(randNum);
                yield return new WaitWhile(() => current);

                Spawn();
            }
        }

        private void Spawn()
        {
            current = Instantiate(mentosPrefab, SpawnTrm.position, Quaternion.identity).GetComponent<Mentos>();
            Material mat = current.GetComponent<MeshRenderer>().material;
            mat.color = Random.ColorHSV();
        }
    }
}
