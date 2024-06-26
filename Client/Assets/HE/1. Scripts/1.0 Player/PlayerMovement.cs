using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HE
{
    public class PlayerMovement : MonoBehaviour
    {
        public static bool canMove = true;

        public PlayerMovementData Data;

        #region COMPONENTS
        public Rigidbody RB { get; private set; }
        public Animator Animator { get; private set; }
        #endregion

        private PlayerDamageble PlayerDamageble;

        #region STATE PARAMETERS
        public bool IsJumping { get; private set; }
        public bool IsRushing { get; private set; }

        private const float LERP_AMOUNT = 1f;
        #endregion

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

        [SerializeField] GameObject PlayerCAM;

        private void Awake()
        {
            RB = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();

            PlayerDamageble = GetComponent<PlayerDamageble>();
            /*DrinkDamageble = GetComponentInChildren<DrinkDamageble>();*/
        }

        private void FixedUpdate()
        {
            // Handle Run
            if (CanRun())
            {
                Run(LERP_AMOUNT);
            }
            // Handle Rush
            if (CanRush())
            {
                Rush();
            }
        }

        private void Update()
        {
            if (!canMove) return;

            #region INPUT HANDLER
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.z = Input.GetAxisRaw("Vertical");

            // jump
            if (CanJump() && Input.GetKeyDown(KeyCode.Space))
            {
                IsJumping = true;

                Jump();
            }
            #endregion
        }

        private void LateUpdate()
        {
            //Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //Vector3 CMAangle = PlayerCAM.transform.rotation.eulerAngles;
            //float x = CMAangle.x - mouseDelta.y;

            //if (x < 180f)
            //{
            //    x = Math.Clamp(x, -1, 70f);
            //}
            //else
            //{
            //    x = Math.Clamp(x, 335f, 360f);
            //}

            //PlayerCAM.transform.rotation = Quaternion.Euler(x, CMAangle.y + mouseDelta.x, CMAangle.z);
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

            if (CanRun())
            {
                RB.velocity = new Vector3(targetSpeed * moveDirection.x, RB.velocity.y, targetSpeed * moveDirection.z);
            }

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
            /*DrinkDamageble.StartRush();*/
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