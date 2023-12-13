using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovementData Data;

    #region COMPONENTS
    public Rigidbody RB { get; private set; }
    #endregion

    private PlayerDamageble PlayerDamageble; //

    #region STATE PARAMETERS
    private const float LERP_AMOUNT = 1f;
    #endregion

    public bool IsJumping { get; private set; }

    #region INPUT PARAMETERS
    public Vector3 _moveInput;
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

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();

        PlayerDamageble = GetComponent<PlayerDamageble>();
    }

    private void FixedUpdate()
    {
        // Handle Run
        if (CanRun())
        {
            // 조금 신경 쓰이지만 일단 해보자...
            Run(LERP_AMOUNT);
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

            Jump();
        }
        #endregion]
    }

    // Movement Methods
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        Vector3 moveDirection = _moveInput.normalized;
        float targetSpeed = moveDirection.magnitude * Data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.magnitude, targetSpeed, lerpAmount);

        RB.velocity = new Vector3(targetSpeed * moveDirection.x, RB.velocity.y, targetSpeed * moveDirection.z);
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
        if (/*만약 러쉬중이 아닐때*/ !PlayerDamageble.IsFainting)
        {
            return true;
        }
        else
            return false;
    }

    private bool CanJump()
    {
        if (IsGounded() && !PlayerDamageble.IsFainting)
            return true;
        else
            return false;
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
            return false;
    }

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_groundCheckPoint.position - _groundCheckPoint.up * _maxDistance, _groundCheckSize);
    }
    #endregion
}