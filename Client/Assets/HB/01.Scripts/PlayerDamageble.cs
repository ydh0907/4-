using AH;
using DH;
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
        private PlayerAttack PlayerAttack;

        public ulong Enemy = ulong.MaxValue;

        public bool IsGroggying { get; private set; }
        [SerializeField] private float _faintTime;
        [SerializeField] private float _respawnTime;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerAttack = GetComponent<PlayerAttack>();
        }

        public void Damage(int damageAmount, Vector3 hitDirection)
        {
            StartCoroutine(nameof(GroggyAction));
        }

        public void Die()
        {
            PlayerAttack.CurrentMentosCount = 0;
            SoundManager.Instance.Play("Effect/DieLaugh");
            IngameUIToolkit.instance.ResurrectionCounter();
            if(Enemy != ulong.MaxValue)
                NetworkGameManager.Instance.PlayerKillCountServerRpc(Enemy);
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