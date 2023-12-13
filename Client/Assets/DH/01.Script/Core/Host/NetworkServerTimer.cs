using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class NetworkServerTimer : NetworkBehaviour
    {
        public static NetworkServerTimer Instance;

        [SerializeField] private float timer = 300;

        public bool Active { get; private set; }

        public override void OnNetworkSpawn()
        {
            Instance = this;

            base.OnNetworkSpawn();
            NetworkGameManager.Instance.onGameStart.AddListener(StartTimer);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            NetworkGameManager.Instance.onGameStart.RemoveListener(StartTimer);
        }

        public void StartTimer()
        {
            Active = true;
        }

        private void Update()
        {
            if (!Active || !IsHost) return;

            timer -= Time.deltaTime;

            if(timer < 0)
            {
                timer = 0;

                //
            }
        }
    }
}
