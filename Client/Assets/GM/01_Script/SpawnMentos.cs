using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class SpawnMentos : MonoBehaviour
    {
        [SerializeField] private GameObject mentosPrefab;
        [SerializeField] private Transform SpawnTrm;

        public static SpawnMentos Instance { get; private set; }

        private int minSpawnTime = 10;
        private int maxSpawnTime = 20;

        private void Awake()
        {
            Instance = this;
        }

        public void StartSpawn()
        {
            StartCoroutine("MentosSpawnCorutine");
        }

        public void StopSpawn()
        {
            StopCoroutine("MentosSpawnCorutine");
        }

        private IEnumerator MentosSpawnCorutine()
        {
            float randNum = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randNum);

            GameObject mentosObj = Instantiate(mentosPrefab, SpawnTrm.position, Quaternion.identity);
            Material mat = mentosObj.GetComponent<MeshRenderer>().material;
            mat.color = Random.ColorHSV();

            yield return new WaitUntil(() => mentosObj == null);
            StartCoroutine("MentosSpawnCorutine");
        }
    }
}
