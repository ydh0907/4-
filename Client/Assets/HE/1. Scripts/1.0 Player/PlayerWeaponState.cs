using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
    public class PlayerWeaponState : MonoBehaviour
    {
        private PlayerMovement PlayerMovement;
        private PlayerKnockback PlayerKnockback;

        private bool isMentosAvailable; // 지금 멘토스를 가지고 있나?

        [SerializeField] private bool IsMisMentos; // 지금 멘토스인가?
        [SerializeField] private bool IsFist; // 지금 멘토스인가?

        private int attackDamageAmount;
        [SerializeField] private int mentosDamageAmount;
        [SerializeField] private int fistDamageAmount;

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
            if (CanChangeToMentosState() && Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnMentosStateActivated();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnFistStateActivated();
            }
            #endregion
        }

        private void OnMentosStateActivated()
        {
            attackDamageAmount = mentosDamageAmount;
            IsMisMentos = true;
            IsFist = false;
        }

        private void OnFistStateActivated()
        {
            attackDamageAmount = fistDamageAmount;
            IsFist = true;
            IsMisMentos = false;
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
                PlayerKnockback.StartKnockback(hitDirection, hitDirection, PlayerMovement._moveInput.x); // 실행 하는지 확인 피ㅣㄹ요
            }
        }

        // ㅔ미지를 불 ㅒ 자시노 함께 ㅇㅇ 맞은 사람이 ㅐ린 사람을 저장 di 함수 실행할 ㅒ 마지막으로 ㅐ린 사람에게 점수 부여
        // 멘토스 상태면 ㅔ미지만 바뀜 코ㅡ 수정
    }
}
