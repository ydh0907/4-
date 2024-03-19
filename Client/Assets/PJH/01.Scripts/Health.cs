using DH;
using System;
using System.Collections;
using System.Collections.Generic;
using HB;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace PJH
{
    public class Health : MonoBehaviour
    {
        public static Health instance = null;

        public int currentHealth;

        //NetworkVariable : ��Ʈ��ũ�� ����� ������ �ν��Ͻ��鳢�� �����ؾ��� ������ ���鶧 ����
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
            currentHealth = MaxHealth;
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
            int prevHealth = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth + value, 0, MaxHealth);
            HealthChangeHandle(prevHealth, currentHealth);
            if (currentHealth == 0)
            {
                _isDead = true;
            }
        }
    }
}