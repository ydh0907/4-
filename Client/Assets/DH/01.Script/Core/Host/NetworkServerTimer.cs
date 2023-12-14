using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkServerTimer : NetworkBehaviour
    {
        public static NetworkServerTimer Instance;

        [SerializeField] private NetworkVariable<float> timer = new(300);

        public bool Active { get; private set; }

        public override void OnNetworkSpawn()
        {
            Instance = this;

            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        public void StartTimer()
        {
            Active = true;
        }

        private void Update()
        {
            if (!Active || !IsHost) return;

            timer.Value -= Time.deltaTime;

            if(timer.Value < 0)
            {
                timer.Value = 0;

                NetworkGameManager.Instance.GameEnd();
            }
        }
    }
}
