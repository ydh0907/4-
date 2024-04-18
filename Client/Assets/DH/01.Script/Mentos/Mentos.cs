using PJH;
using Unity.Netcode;
using UnityEngine;

public class Mentos : NetworkBehaviour
{
    private MentosSpawn spawner;
    private Transform pos;

    public void Init(MentosSpawn mentosSpawn, Transform pos)
    {
        spawner = mentosSpawn;
        this.pos = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.TryGetComponent(out Player player))
        {
            player.AddMentosClientRpc();
            Destroy(gameObject);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
            spawner.SetPositionState(pos);
        base.OnNetworkDespawn();
    }
}
