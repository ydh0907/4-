using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace PJH
{
    public partial class Player : NetworkBehaviour
    {
        public void StartInit()
        {
            base.OnNetworkSpawn();
            Init();
            Join();
            SetStart();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            UnJoin();
        }

        private void SetStart()
        {
            if (_rigidbody.isKinematic) _rigidbody.isKinematic = false;
        }

        private void Join()
        {
            if (!IsOwner) return;
            _inputReader.AttackEvent += HandleAttackEvent;
            _inputReader.MovementEvent += HandleMovementEvent;
            _inputReader.JumpEvent += HandleJumpEvent;
            _inputReader.RunEvent += HandleSprintEvent;
        }

        private void UnJoin()
        {
            if (!IsOwner) return;
            _inputReader.AttackEvent -= HandleAttackEvent;
            _inputReader.MovementEvent -= HandleMovementEvent;
            _inputReader.JumpEvent -= HandleJumpEvent;
            _inputReader.RunEvent -= HandleSprintEvent;
        }

        private void Update()
        {
            if (!IsOwner) return;

            UpdateAnimator();
            CheckGround();
            CheckSlopeLimit();
            UpdateMoveDirection(_mainCamera.transform);

            ControlRotationType();
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

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
            if (_health != null)
            {
                _health.TakeDamage(damage);
                if (_health.CurrentHealth == 0) Death();
            }
        }

        public async void Faint()
        {
            _rigidbody.velocity = Vector3.zero;
            _lockMovement = true;
            _lockRotation = true;
            StopMove = true;
            _inputMagnitude = 0;
            await Task.Delay(500);
            StopMove = false;
            _lockMovement = false;
            _lockRotation = false;
        }

        private void Death()
        {
            if (_isDead) return;
            _isDead = true;
            gameObject.SetActive(false);
            RespawnManager.Instance.Respawn(this);
        }

        public void Respawn()
        {
            IsAttacking = false;
            IsJumping = false;
            _isDead = false;
            IsSprinting = false;
            IsStrafing = false;
            _input = Vector3.zero;
            _lockMovement = false;
            _lockRotation = false;
            StopMove = false;
            gameObject.SetActive(true);
            _rigidbody.position = _respawnPos;
            _health.Reset();
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