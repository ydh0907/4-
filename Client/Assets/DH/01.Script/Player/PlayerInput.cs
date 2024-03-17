using System;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class PlayerInput : NetworkBehaviour
    {
        public static bool Active = true;

        public Action<Vector3> MovementInputAction;
        public Action JumpInputAction;
        public Action AttackInputAction;

        private void Awake()
        {
            // if (!IsOwner) enabled = false;
        }

        private void Update()
        {
            if (Active)
            {
                float x = Input.GetAxisRaw("Horizontal");
                float z = Input.GetAxisRaw("Vertical");

                Vector3 direction = new(x, 0, z);
                MovementInputAction?.Invoke(direction);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    JumpInputAction?.Invoke();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    AttackInputAction?.Invoke();
                }
            }
        }
    }
}
