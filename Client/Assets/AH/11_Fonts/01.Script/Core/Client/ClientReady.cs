using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientReady : NetworkBehaviour
{
    public NetworkVariable<bool> ready = new();
    public ulong ID => NetworkManager.LocalClientId;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsHost) ready.Value = true;
    }
}
