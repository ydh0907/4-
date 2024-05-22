using Cinemachine;
using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        private void Init()
        {
            _cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();

            if (_cinemachineFreeLook.Follow == null)
            {
                if (IsOwner)
                {
                    _cinemachineFreeLook.Follow = transform;
                    _cinemachineFreeLook.LookAt = transform.Find("LookAt");
                    _cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0.002f;
                    _cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0.2f;
                }
            }

            _respawnPos = transform.position;
            DamageCaster = transform.GetComponentInChildren<DamageCaster>();
            _animator = GetComponentInChildren<Animator>();
            _mainCamera = Camera.main;
            _model = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

            _frictionPhysics = new PhysicMaterial();
            _frictionPhysics.name = "_frictionPhysics";
            _frictionPhysics.staticFriction = .25f;
            _frictionPhysics.dynamicFriction = .25f;
            _frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

            _maxFrictionPhysics = new PhysicMaterial();
            _maxFrictionPhysics.name = "_maxFrictionPhysics";
            _maxFrictionPhysics.staticFriction = 1f;
            _maxFrictionPhysics.dynamicFriction = 1f;
            _maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

            _slippyPhysics = new PhysicMaterial();
            _slippyPhysics.name = "_slippyPhysics";
            _slippyPhysics.staticFriction = 0f;
            _slippyPhysics.dynamicFriction = 0f;
            _slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;


            _colliderCenter = _capsuleCollider.center;
            _colliderRadius = _capsuleCollider.radius;
            _colliderHeight = _capsuleCollider.height;

            IsGrounded = true;
            _health = GetComponent<Health>();
            _cinemachineFreeLook.m_XAxis.Value = 0;
            _cinemachineFreeLook.m_YAxis.Value = 1f;
        }

        private void SetAnimatorMoveSpeed(MovementSpeed speed)
        {
            Vector3 relativeInput = transform.InverseTransformDirection(_moveDirection);
            _verticalSpeed = relativeInput.z;
            _horizontalSpeed = relativeInput.x;

            var newInput = new Vector2(_verticalSpeed, _horizontalSpeed);

            if (speed.walkByDefault)
                _inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, IsSprinting ? runningSpeed : walkSpeed);
            else
                _inputMagnitude = Mathf.Clamp(IsSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0,
                    IsSprinting ? sprintSpeed : runningSpeed);
        }
    }
}