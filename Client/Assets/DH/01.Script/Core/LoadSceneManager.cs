using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DH
{
    public class LoadSceneManager : MonoBehaviour
    {
        public static LoadSceneManager Instance;

        public void LoadSceneAsync(int id)
        {
            SceneManager.LoadSceneAsync(id);
        }

        public void LoadSceneAsync(string name)
        {
            SceneManager.LoadSceneAsync(name);
        }

        public void LoadScene(int id)
        {
            SceneManager.LoadScene(id);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void LoadSceneAsync(int id, Action callback)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(id);
            StartCoroutine(WaitCallback(operation, callback));
        }

        public void LoadSceneAsync(string name, Action callback)
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

        public void Quit()
        {
            Application.Quit();
        }
    }
}
