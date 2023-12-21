using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class PlayerWeaponState : MonoBehaviour
    {
        private PlayerMovement PlayerMovement;
        private PlayerKnockback PlayerKnockback;

        private bool isMentosAvailable; // ���� ���佺�� ������ �ֳ�?

        [SerializeField] private bool IsMisMentos; // ���� ���佺�ΰ�?
        [SerializeField] private bool IsFist; // ���� ���佺�ΰ�?

        private int attackDamageAmount;
        public int mentosDamageAmount;
        public int fistDamageAmount;

        private void Awake()
        {
            PlayerMovement = GetComponentInParent<PlayerMovement>();
            PlayerKnockback = GetComponentInParent<PlayerKnockback>();
        }

        private void Update()
        {
            #region INPUT HANDLER
            // Stat
            if (CanChangeToMentosState() && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2)))
            {
                SetToMentosState();
            }
            #endregion
        }

        private void SetToMentosState()
        {
            if (CanChangeToMentosState())
            {
                attackDamageAmount = mentosDamageAmount;
                IsMisMentos = true;
                IsFist = false;
            }
            else
            {
                attackDamageAmount = fistDamageAmount;
                IsFist = true;
                IsMisMentos = false;
            }
        }

        private bool CanChangeToMentosState()
        {
            if (isMentosAvailable)
            {
                return true;
            }
            else
                return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageble iDamageble = other.gameObject.GetComponent<IDamageble>();
            Vector2 hitDirection = other.gameObject.transform.position - transform.position;

            if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("DRINK"))
            {
                iDamageble.Damage(attackDamageAmount, hitDirection);
            }

            else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
            {
                hitDirection.Normalize();
                PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x); // ���� �ϴ��� Ȯ�� �ǤӤ���
            }
        }

        // �Ĺ����� �� �� �ڽó� �Բ� ���� ���� ����� ���� ����� ���� di �Լ� ������ �� ���������� ���� ������� ���� �ο�
        // ���佺 ���¸� �Ĺ����� �ٲ� �ڤ� ����
    }
}
