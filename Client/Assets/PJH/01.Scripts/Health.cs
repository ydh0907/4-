using HB;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

namespace PJH
{
    public class Health : NetworkBehaviour
    {

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
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Reset();
            }

            _health.OnValueChanged += HealthChangeHandle;
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
            if (!IsServer) return;

            if (_isDead) return;
            int prevHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
            Debug.Log("Hit");
            if (CurrentHealth == 0)
            {
                _isDead = true;
            }
        }
    }
}