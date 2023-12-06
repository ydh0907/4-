using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class ColarSystem : MonoBehaviour
    {
        [SerializeField]
        private float rushDecreaseCloarAmount = 1;
        private float currentColarAmount = 1000;
        private Coroutine rushCrountine;

        private void Update()
        {
            Debug.Log(currentColarAmount);
        }

        public void OnDamage(float damage)
        {
            currentColarAmount -= damage;
        }

        public void StartRush()
        {
            rushCrountine = StartCoroutine(Rush());
        }

        public void StopRush()
        {
            StopCoroutine(rushCrountine);
        }

        private IEnumerator Rush()
        {
            while (true)
            {
                OnDamage(rushDecreaseCloarAmount);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
