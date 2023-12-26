using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using GM;

namespace HB
{
    public class PlayerMovement : NetworkBehaviour
    {
        public PlayerMovementData Data;

        #region COMPONENTS
        public Rigidbody RB { get; private set; }
        public Animator Animator { get; private set; }
        #endregion

        private PlayerDamageble PlayerDamageble;
        private DrinkDamageble DrinkDamageble;

        #region STATE PARAMETERS
        private const float LERP_AMOUNT = 1f;
        private const float SPAWN_TIME = 5f;
        #endregion

        public bool IsJumping { get; private set; }
        public bool IsRushing { get; private set; }
        public float CurrentTime { get; private set; }

        #region INPUT PARAMETERS
        [HideInInspector] public Vector3 _moveInput;
        #endregion

        #region CHECK PARAMETERS
        [Header("Checks")]
        [SerializeField] private Transform _groundCheckPoint;
        [Tooltip("groundCheck의 크기는 Player의 크기보다 약간 작은 것이 좋다.")]
        [SerializeField] private Vector3 _groundCheckSize;
        [SerializeField] private float _maxDistance;
        #endregion

        #region LAYERS & TAGS
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask _groundLayer;
        #endregion

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            RB = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();

            PlayerDamageble = GetComponent<PlayerDamageble>();
            DrinkDamageble = GetComponentInChildren<DrinkDamageble>();
        }

        private void FixedUpdate()
        {
            // Handle Run
            if (!IsOwner) return;
            if (CanRun())
            {
                Run(LERP_AMOUNT);
            }
            // rush
            if (CanRush())
            {
                Rush();
            }
        }

        private void Update()
        {
            #region INPUT HANDLER
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.z = Input.GetAxisRaw("Vertical");

            // jump
            if (CanJump() && Input.GetKeyDown(KeyCode.Space))
            {
                IsJumping = true;
                Animator.SetTrigger("IsJumping");

                Jump();
            }
            #endregion

            #region POLAR BEAR SPAWN
            // *주의
            // 기절 시간이 2초, 북극곰 소환 시간이 5초라 하였을 때
            // Player가 3초 동안 가만히 있다가 기절하여도
            // 자의든 타의든 움직이지 않은 시간이 총 5초 이니 북극곰을 소환한다.
            if (RB.velocity != Vector3.zero)
            {
                CurrentTime = 0;
            }
            else
            {
                CurrentTime += Time.deltaTime;
                if (CurrentTime >= SPAWN_TIME)
                {
                    SpawnPolarBear.Instance.CallPolarBear(transform);
                }
            }
            #endregion
        }

        // Movement Methods
        #region RUN METHODS
        private void Run(float lerpAmount)
        {
            Vector3 moveDirection = _moveInput.normalized;
            float targetSpeed;

            if (IsRushing)
                targetSpeed = moveDirection.magnitude * Data.rushMaxSpeed;
            else
                targetSpeed = moveDirection.magnitude * Data.runMaxSpeed;

            targetSpeed = Mathf.Lerp(RB.velocity.magnitude, targetSpeed, lerpAmount);
            Animator.SetFloat("AnimationSpeed", targetSpeed);

            if (!IsOwner) return;

            RB.velocity = new Vector3(targetSpeed * moveDirection.x, RB.velocity.y, targetSpeed * moveDirection.z);

            // Rotation
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), Data.rotationFactorPerFrame * Time.deltaTime);
            }

        }
        #endregion

        #region Rush METHODS
        private void Rush()
        {
            if (!IsOwner) return;
            DrinkDamageble.StartRush();
        }
        #endregion

        #region JUMP METHODS
        private void Jump()
        {
            float force = Data.jumpForce;
            if (RB.velocity.y < 0)
                force -= RB.velocity.y;

            RB.AddForce(Vector2.up * force, ForceMode.Impulse);
        }
        #endregion

        private bool CanRun()
        {
            if (!PlayerDamageble.IsGroggying)
                return true;
            else
            {
                RB.velocity = new Vector3(0f, RB.velocity.y, 0f);
                return false;
            }
        }

        private bool CanRush()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                IsRushing = true;
                return true;
            }
            else
            {
                IsRushing = false;
                return false;
            }
        }

        private bool CanJump()
        {
            if (IsGounded() && !PlayerDamageble.IsGroggying)
                return true;
            else
            {
                return false;
            }
        }

        private bool IsGounded()
        {
            // Ground Check
            if (Physics.BoxCast(_groundCheckPoint.position, _groundCheckSize * 0.5f, -_groundCheckPoint.up, _groundCheckPoint.rotation, _maxDistance, _groundLayer))
            {
                IsJumping = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        #region EDITOR METHODS
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(_groundCheckPoint.position - _groundCheckPoint.up * _maxDistance, _groundCheckSize);
        }
        #endregion
    }
}
