using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DH
{
    public class LoadSceneManager : MonoBehaviour
    {
        public static LoadSceneManager Instance;

        private void Awake()
        {
            if (Instance != null) enabled = false;
            Instance = this;
        }

        public void LoadScene(int id)
        {
            SceneManager.LoadSceneAsync(id);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadSceneAsync(name);
        }

        public void LoadScene(int id, Action callback)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(id);
            StartCoroutine(WaitCallback(operation, callback));
        }

        public void LoadScene(string name, Action callback)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(name);
            StartCoroutine(WaitCallback(operation, callback));
        }

        private IEnumerator WaitCallback(AsyncOperation operation, Action callback)
        {
            while (!operation.isDone)
            {
                yield return null;
            }

            callback?.Invoke();
        }
    }
}
