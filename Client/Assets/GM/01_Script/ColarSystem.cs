using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class ColarSystem : MonoBehaviour
    {
        private float currentColarAmount = 1000;
        private float ruxhDecreaseCloarAmount = 1;
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
                OnDamage(currentColarAmount);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
