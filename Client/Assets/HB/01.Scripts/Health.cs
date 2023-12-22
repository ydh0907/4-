using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace HB
{
    public class Health : NetworkBehaviour
    {
        public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
        //NetworkVariable : ��Ʈ��ũ�� ����� ������ �ν��Ͻ��鳢�� �����ؾ��� ������ ���鶧 ����

        [field: SerializeField]
        public int MaxHealth
        {
            get;
            private set;
        } = 100;

        public Action<Health> OnDie;

        private bool _isDead = false;

        public UnityEvent<int, int, float> OnHealthChanged;

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                currentHealth.OnValueChanged += HealthChangeHandle;

                HealthChangeHandle(0, MaxHealth);
            }

            if (!IsServer) return; // ü�� �ʱ�ȭ�� ������
            currentHealth.Value = MaxHealth;
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
                currentHealth.OnValueChanged -= HealthChangeHandle;
            }
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
            currentHealth.Value = Mathf.Clamp(currentHealth.Value + value, 0, MaxHealth);
            if (currentHealth.Value == 0)
            {
                if (OnDie == null)
                {
                    Debug.Log("empty");
                }

                else
                {
                    OnDie.Invoke(this);
                }

                Debug.Log("A");
                _isDead = true;
            }
        }
    }
}
