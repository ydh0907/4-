using AH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class PlayerWeaponState : MonoBehaviour
    {
        private PlayerMovement PlayerMovement;
        private PlayerKnockback PlayerKnockback;
        private PlayerMentorsManagement PlayerMentorsManagement;

        public bool isMentosAvailable { get; set; } // ���� ���佺�� ������ �ֳ�?
        public bool IsInMentosState { get; set; } // ���� ���佺�ΰ�?

        private int attackDamageAmount;
        [SerializeField] private int mentosDamageAmount;
        [SerializeField] private int fistDamageAmount;

        private void Awake()
        {
            PlayerMovement = GetComponentInParent<PlayerMovement>();
            PlayerKnockback = GetComponentInParent<PlayerKnockback>();
            PlayerMentorsManagement = GetComponentInParent<PlayerMentorsManagement>();
        }

        private void Start()
        {
            OnFistStateActivated();
        }

        private void Update()
        {
            #region INPUT HANDLER
            // Stat
            if (CanChangeToMentosState() && Input.GetKeyDown(KeyCode.Alpha2)) // ���佺
            {
                IngameUIToolkit.instance.ChangeMantosAttack();
                OnMentosStateActivated();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1)) // �ָ�
            {
                IngameUIToolkit.instance.ChangeFistAttack();
                OnFistStateActivated();
            }
            #endregion
        }

        private void OnMentosStateActivated()
        {
            attackDamageAmount = mentosDamageAmount;
            IsInMentosState = true;
        }

        private void OnFistStateActivated()
        {
            attackDamageAmount = fistDamageAmount;
            IsInMentosState = false;
        }

        private bool CanChangeToMentosState()
        {
            if (isMentosAvailable)
                return true;
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

            else if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
            {
                hitDirection.Normalize();
                iDamageble.Damage(attackDamageAmount, hitDirection);
                PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x); // ���� �ϴ��� Ȯ�� �ǤӤ���
            }
        }

        // �Ĺ����� �� �� �ڽó� �Բ� ���� ���� ����� ���� ����� ���� di �Լ� ������ �� ���������� ���� ������� ���� �ο�
    }
}
