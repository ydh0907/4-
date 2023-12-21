using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength;
    [HideInInspector] public float gravityScale;

    [Space(5)]
    [Header("Gravity")]
    [Tooltip("�ִ� �߷� ��")]
    public float fallGravityMult;

    [Space(20)]

    [Header("Run")]
    [Tooltip("�ִ� �̵� �ӵ�")]
    public float runMaxSpeed;

    [Space(20)]

    [Header("Rush")]
    public float rushMaxSpeed;

    [Space(20)]

    [Header("Jump")]
    [Tooltip("�ִ� ���� ����")]
    public float jumpHeight;
    [Tooltip("�ִ� ���� ���̱��� �����ϴ� �ð�")]
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
