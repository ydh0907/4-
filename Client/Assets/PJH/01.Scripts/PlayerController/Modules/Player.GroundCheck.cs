using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        #region Ground Check

        private void CheckGround()
        {
            CheckGroundDistance();
            ControlMaterialPhysics();

            if (_groundDistance <= _groundMinDistance)
            {
                IsGrounded = true;
                if (!IsJumping && _groundDistance > 0.05f)
                    _rigidbody.AddForce(transform.up * (_extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);

                _heightReached = transform.position.y;
            }
            else
            {
                if (_groundDistance >= _groundMaxDistance)
                {
                    IsGrounded = false;
                    if (!IsJumping)
                    {
                        _rigidbody.AddForce(transform.up * (_extraGravity * Time.deltaTime), ForceMode.VelocityChange);
                    }
                }
                else if (!IsJumping)
                {
                    _rigidbody.AddForce(transform.up * (_extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
                }
            }
        }

        private void ControlMaterialPhysics()
        {
            _capsuleCollider.material =
                (IsGrounded && GroundAngle() <= _slopeLimit + 1) ? _frictionPhysics : _slippyPhysics;

            if (IsGrounded && _input == Vector3.zero)
                _capsuleCollider.material = _maxFrictionPhysics;
            else if (IsGrounded && _input != Vector3.zero)
                _capsuleCollider.material = _frictionPhysics;
            else
                _capsuleCollider.material = _slippyPhysics;
        }

        private void CheckGroundDistance()
        {
            if (_capsuleCollider != null)
            {
                float radius = _capsuleCollider.radius * 0.9f;
                var dist = 10f;
                Ray ray2 = new Ray(transform.position + new Vector3(0, _colliderHeight / 2, 0), Vector3.down);
                if (Physics.Raycast(ray2, out _groundHit, (_colliderHeight / 2) + dist, _whatIsGround) &&
                    !_groundHit.collider.isTrigger)
                    dist = transform.position.y - _groundHit.point.y;
                if (dist >= _groundMinDistance)
                {
                    Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
                    Ray ray = new Ray(pos, -Vector3.up);
                    if (Physics.SphereCast(ray, radius, out _groundHit, _capsuleCollider.radius + _groundMaxDistance,
                            _whatIsGround) && !_groundHit.collider.isTrigger)
                    {
                        Physics.Linecast(_groundHit.point + (Vector3.up * 0.1f),
                            _groundHit.point + Vector3.down * 0.15f,
                            out _groundHit, _whatIsGround);
                        float newDist = transform.position.y - _groundHit.point.y;
                        if (dist > newDist) dist = newDist;
                    }
                }

                _groundDistance = (float)System.Math.Round(dist, 2);
            }
        }

        private float GroundAngle()
        {
            var groundAngle = Vector3.Angle(_groundHit.normal, Vector3.up);
            return groundAngle;
        }

        private float GroundAngleFromDirection()
        {
            var dir = IsStrafing && _input.magnitude > 0
                ? (transform.right * _input.x + transform.forward * _input.z).normalized
                : transform.forward;
            var movementAngle = Vector3.Angle(dir, _groundHit.normal) - 90;
            return movementAngle;
        }

        #endregion
    }
}