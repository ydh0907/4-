using PJH;
using Unity.Netcode;
using UnityEngine;

public class Mentos : NetworkBehaviour
{
    private MentosSpawn spawner;

    public void Init(MentosSpawn mentosSpawn)
    {
        spawner = mentosSpawn;
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

    public override void OnNetworkSpawn()
    {
        Debug.Log("Mentos Spawned");
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
            spawner.SetPositionState(transform);
        base.OnNetworkDespawn();
    }
}
