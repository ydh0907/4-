using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HB
{
    public class DrinkDamageble : MonoBehaviour, IDamageble
    {
        private PlayerDamageble PlayerDamageble;

        [field: SerializeField] public int CurrentHealth { get; set; }
        [field: SerializeField] public int MaxHealth { get; set; }

        ulong Enemy
        {
            get
            {
                return PlayerDamageble.Enemy;
            }
            set
            {
                PlayerDamageble.Enemy = value;
            }
        }

        private void Awake()
        {
            PlayerDamageble = GetComponentInParent<PlayerDamageble>();
        }

        private void Start()
        {
            MaxHealth = 1000;
            CurrentHealth = MaxHealth;
        }

        public void Damage(int damageAmount, Vector3 hitDirection)
        {
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void StartRush()
        {
            StartCoroutine(nameof(RushDamage));
        }

        public void Die()
        {
            PlayerDamageble.Die();
        }

        IEnumerator RushDamage()
        {
            CurrentHealth--;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("WEAPON"))
            {
                Enemy = other.transform.root.GetComponent<NetworkObject>().OwnerClientId;
            }
        }
    }
}