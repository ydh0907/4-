using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HB
{
    public class KillCount : NetworkBehaviour
    {
        public NetworkVariable<int> killCount = new NetworkVariable<int>();

        private void Awake()
        {
            if (!IsOwner) return;
            killCount.Value = 0;
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
            }
        }

        public void UpdateKillCount(Health health)
        {
            killCount.Value += 1;
            Debug.Log($"{health.name}, {killCount.Value}");
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
            }
        }
    }
}
