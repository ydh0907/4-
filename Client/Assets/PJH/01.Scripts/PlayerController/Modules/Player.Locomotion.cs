using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        #region Locomotion

        private void SetControllerMoveSpeed(MovementSpeed speed)
        {
            if (speed.walkByDefault)
                _moveSpeed = Mathf.Lerp(_moveSpeed, IsSprinting ? speed.runningSpeed : speed.walkSpeed,
                    speed.movementSmooth * Time.deltaTime);
            else
                _moveSpeed = Mathf.Lerp(_moveSpeed, IsSprinting ? speed.sprintSpeed : speed.runningSpeed,
                    speed.movementSmooth * Time.deltaTime);
        }

        private void MoveCharacter(Vector3 _direction)
        {
            _inputSmooth = Vector3.Lerp(_inputSmooth, _input,
                (IsStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.fixedDeltaTime);

            if (!IsGrounded || IsJumping) return;

            _direction.y = 0;
            _direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
            _direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
            // limit the _input
            if (_direction.magnitude > 1f)
                _direction.Normalize();

            Vector3 targetPosition = (UseRootMotion ? _animator.rootPosition : transform.position) +
                                     _direction * ((StopMove ? 0 : _moveSpeed) * Time.fixedDeltaTime);
            Vector3 targetVelocity = (targetPosition - transform.position) / Time.fixedDeltaTime;

            bool useVerticalVelocity = true;
            if (useVerticalVelocity) targetVelocity.y = _rigidbody.velocity.y;

            _rigidbody.velocity = targetVelocity;
        }

        private void CheckSlopeLimit()
        {
            if (_input.sqrMagnitude < 0.1) return;

            RaycastHit hitinfo;
            var hitAngle = 0f;

            if (Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f),
                    transform.position + _moveDirection.normalized * (_capsuleCollider.radius + 0.2f), out hitinfo,
                    _whatIsGround))
            {
                hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                var targetPoint = hitinfo.point + _moveDirection.normalized * _capsuleCollider.radius;
                if ((hitAngle > _slopeLimit) &&
                    Physics.Linecast(transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), targetPoint,
                        out hitinfo, _whatIsGround))
                {
                    hitAngle = Vector3.Angle(Vector3.up, hitinfo.normal);

                    if (hitAngle > _slopeLimit && hitAngle < 85f)
                    {
                        StopMove = true;
                        return;
                    }
                }
            }

            StopMove = false;
        }

        private void RotateToPosition(Vector3 position)
        {
            Vector3 desiredDirection = position - transform.position;
            RotateToDirection(desiredDirection.normalized);
        }

        private void RotateToDirection(Vector3 direction)
        {
            RotateToDirection(direction, IsStrafing ? strafeSpeed.rotationSpeed : freeSpeed.rotationSpeed);
        }

        private void RotateToDirection(Vector3 direction, float rotationSpeed)
        {
            if (!_jumpAndRotate && !IsGrounded) return;
            direction.y = 0f;
            Vector3 desiredForward =
                Vector3.RotateTowards(_model.transform.forward, direction.normalized,
                    rotationSpeed * Time.fixedDeltaTime,
                    .1f);
            Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
            _model.transform.rotation = _newRotation;
        }

        private void ControlLocomotionType()
        {
            if (_lockMovement) return;

            if (!IsStrafing)
            {
                SetControllerMoveSpeed(freeSpeed);
                SetAnimatorMoveSpeed(freeSpeed);
            }
            else
            {
                IsStrafing = true;
                SetControllerMoveSpeed(strafeSpeed);
                SetAnimatorMoveSpeed(strafeSpeed);
            }

            if (!UseRootMotion)
                MoveCharacter(_moveDirection);
        }

        private void ControlRotationType()
        {
            if (_lockRotation) return;

            bool validInput = _input != Vector3.zero ||
                              (IsStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                _inputSmooth = Vector3.Lerp(_inputSmooth, _input,
                    (IsStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                Vector3 dir =
                    (IsStrafing && (!IsSprinting || _sprintOnlyFree == false) ||
                     (freeSpeed.rotateWithCamera && _input == Vector3.zero)) && _rotateTarget
                        ? _rotateTarget.forward
                        : _moveDirection;
                RotateToDirection(dir);
            }
        }

        #endregion
    }
}