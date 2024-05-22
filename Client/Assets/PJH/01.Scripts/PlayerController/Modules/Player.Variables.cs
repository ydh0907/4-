using Cinemachine;
using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        [SerializeField] private InputReader _inputReader;

        #region Movement

        [Header("Movement")][SerializeField] private bool _rotateByWorld = false;
        [SerializeField] private bool _useContinuousSprint = true;
        [SerializeField] private bool _sprintOnlyFree = true;

        [SerializeField] private MovementSpeed freeSpeed, strafeSpeed;

        #endregion

        #region Airborne

        [Header("Airborne")][SerializeField] private bool _jumpWithRigidbodyForce = false;
        [SerializeField] private bool _jumpAndRotate = true;
        [SerializeField] private float _jumpTimer = 0.3f;
        [SerializeField] private float _jumpHeight = 4f;
        [SerializeField] private float _airSpeed = 5f;
        [SerializeField] private float _airSmooth = 6f;
        [SerializeField] private float _extraGravity = -10f;

        #endregion

        #region Ground

        [Header("Ground")][SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _groundMinDistance = 0.25f;
        [SerializeField] private float _groundMaxDistance = 0.5f;
        [Range(30, 80)][SerializeField] private float _slopeLimit = 75f;

        #endregion

        #region Components

        private PhysicMaterial
            _frictionPhysics, _maxFrictionPhysics, _slippyPhysics; // create PhysicMaterial for the Rigidbody

        private Camera _mainCamera;

        private Animator _animator;
        private Rigidbody _rigidbody;
        private Transform _model;
        private CapsuleCollider _capsuleCollider;
        public CapsuleCollider CapsuleCollider => _capsuleCollider;

        public DamageCaster DamageCaster { get; private set; }
        public Animator Animator => _animator;

        private Health _health;

        public Transform Model => _model;

        #endregion

        #region Animation

        private const float walkSpeed = 0.5f;
        private const float runningSpeed = 1f;
        private const float sprintSpeed = 1.5f;

        #region Hash

        private static readonly int InputMagnitudeHash = Animator.StringToHash("InputMagnitude");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
        private static readonly int IsSprintingHash = Animator.StringToHash("IsSprinting");
        private static readonly int GroundDistanceHash = Animator.StringToHash("GroundDistance");

        #endregion

        #endregion


        public bool IsAttacking { get; set; }
        public bool IsJumping { get; set; }

        public bool IsStrafing { get; set; }

        public bool IsGrounded { get; set; }
        public bool IsSprinting { get; set; }
        public bool StopMove { get; set; }

        public bool UseRootMotion { get; set; }

        private float _inputMagnitude;
        private float _verticalSpeed;
        private float _horizontalSpeed;
        private float _moveSpeed;
        private float _verticalVelocity;
        private float _colliderRadius, _colliderHeight;
        private float _heightReached;
        private float _jumpCounter;
        private float _groundDistance;
        private RaycastHit _groundHit;
        public bool _lockMovement = false;
        public bool _lockRotation = false;
        private Transform _rotateTarget;
        private Vector3 _input;
        private Vector3 _colliderCenter;
        private Vector3 _inputSmooth;
        private Vector3 _moveDirection;
        private Vector3 _respawnPos;
        private bool _isDead;
        private CinemachineFreeLook _cinemachineFreeLook;

        [System.Serializable]
        public class MovementSpeed
        {
            [Range(1f, 20f)] public float movementSmooth = 6f;
            [Range(0f, 1f)] public float animationSmooth = 0.2f;
            public float rotationSpeed = 16f;
            public bool walkByDefault = false;
            public bool rotateWithCamera = false;
            public float walkSpeed = 2f;
            public float runningSpeed = 4f;
            public float sprintSpeed = 6f;
        }
    }
}