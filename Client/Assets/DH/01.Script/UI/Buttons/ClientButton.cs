using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace DH
{
    public class ClientButton : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitWhile(() => NetworkManager.Singleton == null);

            GetComponent<Button>().onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        }
    }
}
