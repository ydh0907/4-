using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        private void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (_input.magnitude <= 0.01)
            {
                _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero,
                    (IsStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !_rotateByWorld)
            {
                var right = referenceTransform.right;
                right.y = 0;
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                _moveDirection = (_inputSmooth.x * right) + (_inputSmooth.z * forward);
            }
            else
            {
                _moveDirection = new Vector3(_inputSmooth.x, 0, _inputSmooth.z);
            }
        }

        private void UpdateAnimator()
        {
            if (_animator == null || !_animator.enabled) return;


            _animator.SetBool(IsSprintingHash, IsSprinting);
            _animator.SetBool(IsGroundedHash, IsGrounded);
            _animator.SetFloat(GroundDistanceHash, _groundDistance);


            _animator.SetFloat(InputMagnitudeHash, StopMove ? 0f : _inputMagnitude,
                IsStrafing ? strafeSpeed.animationSmooth : freeSpeed.animationSmooth, Time.deltaTime);
        }
    }
}