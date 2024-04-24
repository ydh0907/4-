using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class Attack : MonoBehaviour
    {
        public static Attack Instance;

        public bool isAttack { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public bool IsAttack()
        {
            return isAttack = true;
        }

        public bool NotAttack()
        {
            return isAttack = false;
        }
    }
}