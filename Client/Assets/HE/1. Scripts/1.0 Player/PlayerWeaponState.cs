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

        public bool isMentosAvailable { get; set; } // ���� ���佺�� ������ �ֳ�?
        public bool IsInMentosState { get; set; } // ���� ���佺�ΰ�?

        public int mentosDamageAmount = 40;
        public int fistDamageAmount = 15;

        private int attackDamageAmount;

        private void Awake()
        {
            PlayerMovement = GetComponentInParent<PlayerMovement>();
            PlayerKnockback = GetComponentInParent<PlayerKnockback>();
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

            if (!isActiveAndEnabled)
            {
                IngameUIToolkit.instance.ChangeFistAttack();
                OnFistStateActivated();
            }
        }

        private bool OnMentosStateActivated()
        {
            return IsInMentosState = true;
        }

        private bool OnFistStateActivated()
        {
            return IsInMentosState = false;
        }

        private bool CanChangeToMentosState()
        {
            if (isMentosAvailable)
                return true;
            else
                return false;
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody is null) return;

            if (other.attachedRigidbody.TryGetComponent<IDamageble>(out IDamageble damageble))
            {
                Vector2 hitDirection = other.gameObject.transform.position - transform.position;

                if (other.gameObject.layer == LayerMask.NameToLayer("DRINK"))
                {
                    damageble.Damage(attackDamageAmount, hitDirection);
                }

                else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
                {
                    hitDirection.Normalize();
                    damageble.Damage(attackDamageAmount, hitDirection);
                    PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x); // ���� �ϴ��� Ȯ�� �ǤӤ���
                }
            }
        }*/

        // �Ĺ����� �� �� �ڽó� �Բ� ���� ���� ����� ���� ����� ���� di �Լ� ������ �� ���������� ���� ������� ���� �ο�
    }
}
