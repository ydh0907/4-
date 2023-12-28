using AH;
using Cinemachine;
using GM;
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
        private CinemachineFreeLook CinemachineFreeLook;

        public bool IsGroggying { get; private set; }
        [SerializeField] private float _faintTime;
        [SerializeField] private float _respawnTime;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerAttack = GetComponent<PlayerAttack>();
            CinemachineFreeLook = GetComponentInChildren<CinemachineFreeLook>();
        }

        public void Damage(int damageAmount, Vector3 hitDirection)
        {
            StartCoroutine(nameof(GroggyAction));
        }

        public void Die()
        {
            PlayerAttack.CurrentMentosCount = 0;
            SoundManager.Instance.Play("Effect/DieLaugh");
            StartCoroutine(nameof(PlayerRespawn));
        }

        IEnumerator GroggyAction()
        {
            IsGroggying = true;
            yield return new WaitForSeconds(_faintTime);
            IsGroggying = false;
        }

        IEnumerator PlayerRespawn() // ������
        {
            Debug.Log("respawn wait");
            IngameUIToolkit.instance.ResurrectionCounter();
            CinemachineFreeLook.Priority = -1; // ī�޶� �̵�
            transform.position = new Vector3(0, 0, -10);

            yield return new WaitForSeconds(_respawnTime);

            CinemachineFreeLook.Priority = 1; // ������� ������
            Health.instance.RestoreHealth(100);
            transform.position = MapManager.Instance.GetSpawnPosition(); // ���� ����Ʈ ����
            Debug.Log("end");
        }
    }
}