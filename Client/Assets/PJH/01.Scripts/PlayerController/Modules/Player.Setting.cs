using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        private void Init()
        {
            _model = transform.Find("Model");
            _mainCamera = Camera.main;
            _animator = _model.GetComponent<Animator>();
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

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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