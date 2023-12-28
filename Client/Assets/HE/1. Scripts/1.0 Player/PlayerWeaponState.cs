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

        public bool isMentosAvailable { get; set; } // 지금 멘토스를 가지고 있나?
        public bool IsInMentosState { get; set; } // 지금 멘토스인가?

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
            if (CanChangeToMentosState() && Input.GetKeyDown(KeyCode.Alpha2)) // 맨토스
            {
                IngameUIToolkit.instance.ChangeMantosAttack();
                OnMentosStateActivated();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1)) // 주먹
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
                    PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x); // 실행 하는지 확인 피ㅣㄹ요
                }
            }
        }*/

        // ㅔ미지를 불 ㅒ 자시노 함께 ㅇㅇ 맞은 사람이 ㅐ린 사람을 저장 di 함수 실행할 ㅒ 마지막으로 ㅐ린 사람에게 점수 부여
    }
}
