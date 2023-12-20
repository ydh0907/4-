using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength;
    [HideInInspector] public float gravityScale;

    [Space(5)]
    [Header("Gravity")]
    [Tooltip("최대 중력 값")]
    public float fallGravityMult;

    [Space(20)]

    [Header("Run")]
    [Tooltip("최대 이동 속도")]
    public float runMaxSpeed;

    [Space(20)]

    [Header("Rush")]
    public float rushMaxSpeed;

    [Space(20)]

    [Header("Jump")]
    [Tooltip("최대 점프 높이")]
    public float jumpHeight;
    [Tooltip("최대 점프 높이까지 도달하는 시간")]
    public float jumpTimeToApex; 
    [HideInInspector] public float jumpForce;

    [Space(20)]
    public float rotationFactorPerFrame;

    private void OnValidate()
    {
        // Gravity
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        gravityScale = gravityStrength / Physics.gravity.y;

        // Jump
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
    }
}
