using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        #region Jump Methods

        private void ControlJumpBehaviour()
        {
            if (!IsJumping) return;

            _jumpCounter -= Time.deltaTime;
            if (_jumpCounter <= 0)
            {
                _jumpCounter = 0;
                IsJumping = false;
            }

            // apply extra force to the jump height   
            var vel = _rigidbody.velocity;
            vel.y = _jumpHeight;
            _rigidbody.velocity = vel;
        }

        private void AirControl()
        {
            if ((IsGrounded && !IsJumping)) return;
            if (transform.position.y > _heightReached) _heightReached = transform.position.y;
            _inputSmooth = Vector3.Lerp(_inputSmooth, _input, _airSmooth * Time.fixedDeltaTime);

            if (_jumpWithRigidbodyForce && !IsGrounded)
            {
                _rigidbody.AddForce(_moveDirection * (_airSpeed * Time.fixedDeltaTime), ForceMode.VelocityChange);
                return;
            }

            _moveDirection.y = 0;
            _moveDirection.x = Mathf.Clamp(_moveDirection.x, -1f, 1f);
            _moveDirection.z = Mathf.Clamp(_moveDirection.z, -1f, 1f);

            Vector3 targetPosition = _rigidbody.position + _moveDirection * (_airSpeed * Time.fixedDeltaTime);
            Vector3 targetVelocity = (targetPosition - transform.position) / Time.fixedDeltaTime;

            targetVelocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, _airSmooth * Time.fixedDeltaTime);
        }

        private bool jumpFwdCondition
        {
            get
            {
                Vector3 p1 = transform.position + _capsuleCollider.center +
                             Vector3.up * -_capsuleCollider.height * 0.5F;
                Vector3 p2 = p1 + Vector3.up * _capsuleCollider.height;
                return Physics.CapsuleCastAll(p1, p2, _capsuleCollider.radius * 0.5f, transform.forward, 0.6f,
                        _whatIsGround)
                    .Length == 0;
            }
        }

        #endregion
    }
}