using System;
using Unity.VisualScripting;
using UnityEngine;

namespace PJH
{
    public partial class Player : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            if (_rigidbody.isKinematic) _rigidbody.isKinematic = false;
        }

        private void OnEnable()
        {
            _inputReader.MovementEvent += HandleMovementEvent;
            _inputReader.JumpEvent += HandleJumpEvent;
            _inputReader.RunEvent += HandleSprintEvent;
            _inputReader.AttackEvent += HandleAttackEvent;
        }

        private void OnDisable()
        {
            _inputReader.MovementEvent -= HandleMovementEvent;
            _inputReader.JumpEvent -= HandleJumpEvent;
            _inputReader.RunEvent -= HandleSprintEvent;
            _inputReader.AttackEvent -= HandleAttackEvent;
        }

        private void Update()
        {
            CheckGround();
            CheckSlopeLimit();
            UpdateAnimator();
            UpdateMoveDirection(_mainCamera.transform);

            ControlRotationType();
        }

        private void FixedUpdate()
        {
            ControlJumpBehaviour();
            ControlLocomotionType();
            AirControl();
        }

        #region Handle event

        private void HandleMovementEvent(Vector3 input)
        {
            _input = input;
        }

        private void HandleJumpEvent()
        {
            if (IsGrounded && GroundAngle() < _slopeLimit && !IsJumping && !StopMove)
                Jump();
        }

        private void HandleSprintEvent(bool isSprint)
        {
            Sprint(isSprint);
        }

        private void HandleAttackEvent()
        {
            if (IsAttacking) return;
            IsAttacking = true;
            _animator.CrossFadeInFixedTime("Attack", .1f);
        }

        #endregion

        public void FinishAttack()
        {
            IsAttacking = false;
        }
    }
}