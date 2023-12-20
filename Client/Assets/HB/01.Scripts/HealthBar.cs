using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class HealthBar : MonoBehaviour
    {
        [Header("참조용 변수들")]
        [SerializeField]
        private Transform _barTransform;

        public void HandleHealthChanged(int oldHealth, int newHealth, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            _barTransform.localScale = new Vector3(ratio, 1, 1);
        }
    }
}
