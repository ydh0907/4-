using HB;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

namespace PJH
{
    public class Health : NetworkBehaviour
    {
        private NetworkVariable<float> _health = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public float CurrentHealth
        {
            get { return _health.Value; }
            set { _health.Value = value; }
        }

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;

        private bool _isDead = false;

        public UnityEvent<float, float, float> OnHealthChanged;

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

        private void HealthChangeHandle(float prev, float newValue)
        {
            OnHealthChanged?.Invoke(prev, newValue, newValue / MaxHealth);
        }

        public void TakeDamage(float damageValue)
        {
            ModifyHealth(-damageValue);
        }

        public void RestoreHealth(float healValue)
        {
            ModifyHealth(healValue);
        }

        private void ModifyHealth(float value)
        {
            if (!IsServer) return;

            if (_isDead) return;
            float prevHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
            if (CurrentHealth <= Mathf.Epsilon)
            {
                _isDead = true;
            }
        }
    }
}