using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class SpawnMentos : MonoBehaviour
    {
        [SerializeField] private GameObject mentosPrefab;
        [SerializeField] private Transform SpawnTrm;

        private void Start()
        {
            StartCoroutine("MentosSpawnCorutine");
        }

        private IEnumerator MentosSpawnCorutine()
        {
            float randNum = Random.Range(25, 35);
            GameObject mantosObj = Instantiate(mentosPrefab, SpawnTrm.position, Quaternion.identity);
            Material mat = mantosObj.GetComponent<MeshRenderer>().material;
            mat.color = Random.ColorHSV();

            yield return new WaitUntil(() => transform.GetChild(0) != null);
            yield return new WaitForSeconds(randNum);

            StartCoroutine("MentosSpawnCorutine");
        }
    }
}
