using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using GM;
using System;

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

        float targetSpeed;

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

        [SerializeField] Transform follow;

        Vector3 temppos = Vector3.zero;

        public override void OnNetworkSpawn()
        {
            RB = GetComponent<Rigidbody>();

            PlayerDamageble = GetComponent<PlayerDamageble>();
            SetGravityScale(Data.gravityScale);

            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            if(!IsOwner) return;

            if (!Animator || !DrinkDamageble)
            {
                return;
            }

            if (CanRun())
            {
                Run(LERP_AMOUNT);
            }
            // rush
            if (CanRush())
            {
                Rush();
            }

            if (RB.velocity.y < 0)
            {
                // 낙하시 중력값 증가 
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            }
            else
            {
                // 기본 중력값
                SetGravityScale(Data.gravityScale);
            }
        }

        private void Update()
        {
            if (!Animator || !DrinkDamageble)
            {
                Animator = GetComponentInChildren<Animator>();
                DrinkDamageble = GetComponentInChildren<DrinkDamageble>();

                return;
            }

            if (!IsOwner)
            {
                SetAnimation();
                return;
            }

            #region INPUT HANDLER
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            _moveInput.x = x;
            _moveInput.z = z;

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
                    CurrentTime = 0;
                }
            }
            #endregion
        }

        private void SetAnimation()
        {
            if(transform.position != temppos && transform.position.y == temppos.y)
            {
                Animator.SetFloat("AnimationSpeed", targetSpeed);
                temppos = transform.position;
            }
            else if(!Physics.Raycast(transform.position, Vector3.down, 0.6f, _groundLayer))
            {
                Animator.SetTrigger("IsJumping");
            }
        }

        public void SetGravityScale(float scale)
        {
            RB.AddForce(Vector3.down * scale);
        }

        // Movement Methods
        #region RUN METHODS
        private void Run(float lerpAmount)
        {
            Vector3 moveDirection = _moveInput.normalized;

            if (IsRushing)
                targetSpeed = moveDirection.magnitude * Data.rushMaxSpeed;
            else
                targetSpeed = moveDirection.magnitude * Data.runMaxSpeed;

            targetSpeed = Mathf.Lerp(RB.velocity.magnitude, targetSpeed, lerpAmount);
            Animator.SetFloat("AnimationSpeed", targetSpeed);

            if (CanRun())
            {
                Vector3 velo = (follow.right * moveDirection.x + follow.forward * moveDirection.z).normalized * targetSpeed;
                RB.velocity = new Vector3(velo.x, RB.velocity.y, velo.z);
            }

            // Rotation
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(RB.velocity.normalized), Data.rotationFactorPerFrame * Time.deltaTime);
            }
        }
        #endregion

        #region Rush METHODS
        private void Rush()
        {
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
