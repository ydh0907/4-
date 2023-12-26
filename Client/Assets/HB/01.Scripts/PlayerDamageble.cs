using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class PlayerDamageble : MonoBehaviour, IDamageble
    {
        public int CurrentHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private PlayerMovement PlayerMovement;

        public bool IsGroggying { get; private set; }
        [SerializeField] private float _faintTime;
        [SerializeField] private float _respawnTime;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
        }

        public void Damage(int damageAmount, Vector3 hitDirection)
        {
            StartCoroutine(nameof(GroggyAction));
        }

        public void Die()
        {
            StartCoroutine(nameof(PlayerRespawn));
        }

        IEnumerator GroggyAction()
        {
            IsGroggying = true;
            yield return new WaitForSeconds(_faintTime);
            IsGroggying = false;
        }

        IEnumerator PlayerRespawn()
        {
            yield return new WaitForSeconds(_respawnTime);
            // ¸®½ºÆù
        }
    }
}