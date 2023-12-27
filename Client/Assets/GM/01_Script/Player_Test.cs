using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GM
{
    public class Player_Test : MonoBehaviour
    {
        [SerializeField] Transform[] trm;
        [SerializeField] GameObject obj;

        private void Start()
        {
            //StartCoroutine("Test");
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RankingPodium.Instance.SetPlayerPodium(trm);
            }
        }

        IEnumerator Test()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                Vector3 pos = MapManager.Instance.GetSpawnPosition();
                Debug.Log(pos);
                Instantiate(obj).transform.position = pos;
            }
        }
    }
}
