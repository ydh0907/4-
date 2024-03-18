using System;
using System.Threading.Tasks;
using HB;
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

        #region Debugging

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_capsuleCollider == null) _capsuleCollider = GetComponent<CapsuleCollider>();
            float radius = _capsuleCollider.radius * 0.9f;
            Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos + (-Vector3.up * (_capsuleCollider.radius + _groundMaxDistance)), radius);
            Gizmos.color = Color.white;
        }
#endif

        #endregion


        public async void ApplyDamage(int damage)
        {
            if (!IsAttacking)
                _animator.Play("Hit");
            if (Health.instance != null)
            {
                Health.instance.TakeDamage(damage);
                if (Health.instance.currentHealth.Value == 0) Death();
            }
        }

        public async void Faint()
        {
            _rigidbody.velocity = Vector3.zero;
            _lockMovement = true;
            _lockRotation = true;
            StopMove = true;
            await Task.Delay(500);
            StopMove = false;
            _lockMovement = false;
            _lockRotation = false;
        }

        private void Death()
        {
        }

        public async void AddForce(Vector3 dir)
        {
            StopMove = true;
            _lockMovement = true;
            _lockRotation = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(dir, ForceMode.Impulse);
            await Task.Delay(500);
            StopMove = false;
            _lockMovement = false;
            _lockRotation = false;
        }
    }
}