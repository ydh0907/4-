using Unity.Netcode;
using UnityEngine;

public class MentosSinMovement : NetworkBehaviour
{
    [SerializeField] private float yMultiply = 1f;
    [SerializeField] private float timeMultiply = 1f;

    private Vector3 originPos;

    public override void OnNetworkSpawn()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if (!IsServer) return;

        float moveDelta;

        moveDelta = Mathf.Sin((float)NetworkManager.Singleton.ServerTime.Time * timeMultiply) * yMultiply;

        transform.position = new Vector3(originPos.x, originPos.y + moveDelta, originPos.z);
    }
}
