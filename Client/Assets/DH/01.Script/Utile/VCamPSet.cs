using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class VCamPSet : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsOwner)
            GetComponent<CinemachineFreeLook>().Priority = 10;
    }
}
