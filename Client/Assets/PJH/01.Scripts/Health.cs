using HB;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace PJH
{
    public class Health : NetworkBehaviour
    {
        public static Health instance = null;

        private NetworkVariable<int> _health = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Reset();
            }
        }

        public void Reset()
        {
            _isDead = false;
            CurrentHealth = MaxHealth;
            HealthChangeHandle(MaxHealth, MaxHealth);
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
            Debug.Log("Hit");
            if (CurrentHealth == 0)
            {
                _isDead = true;
            }
        }
    }
}