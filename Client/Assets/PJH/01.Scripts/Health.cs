using HB;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace PJH
{
    public class Health : NetworkBehaviour
    {
        public static Health instance = null;

        private NetworkVariable<int> _health = new NetworkVariable<int> { Value = 0 };

        public int CurrentHealth
        {
            get
            {
                return _health.Value;
            }
            set
            {
                _health.Value = value;
            }
        }

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;

        private bool _isDead = false;

        public UnityEvent<int, int, float> OnHealthChanged;

        private HealthBar _healthBar;

        private void Awake()
        {
            _healthBar = GetComponent<HealthBar>();
            if (instance == null)
            {
                instance = this;
            }

            OnHealthChanged.AddListener(_healthBar.HandleHealthChanged);
            Reset();
        }

        public void Reset()
        {
            HealthChangeHandle(MaxHealth, MaxHealth);

            _isDead = false;
            CurrentHealth = MaxHealth;
        }

        private void HealthChangeHandle(int prev, int newValue)
        {
            OnHealthChanged?.Invoke(prev, newValue, (float)newValue / MaxHealth);
        }

        public void TakeDamage(int damageValue)
        {
            ModifyHealth(-damageValue);
        }

        public void RestoreHealth(int healValue)
        {
            ModifyHealth(healValue);
        }

        private void ModifyHealth(int value)
        {
            if (_isDead) return;
            int prevHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
            HealthChangeHandle(prevHealth, CurrentHealth);
            if (CurrentHealth == 0)
            {
                _isDead = true;
            }
        }
    }
}