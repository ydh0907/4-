using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HB
{
    public class KillCount : NetworkBehaviour
    {
        public NetworkVariable<int> killCount = new NetworkVariable<int>();
        Health health;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            health = GetComponent<Health>();
            
            killCount.Value = 0;

        }

        public void UpdateKillCount(Health health)
        {
            killCount.Value += 1;

            SoundManager.Instance.Play("Kill");
        }

        public override void OnNetworkDespawn()
        {
        }
    }
}
