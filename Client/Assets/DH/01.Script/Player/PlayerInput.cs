using UnityEngine;
using Unity.Netcode;
using Microsoft.Win32.SafeHandles;

namespace DH
{
    public class PlayerInput : NetworkBehaviour
    {
        public static PlayerInput Instance;

        private void Awake()
        {
            if (!IsOwner) enabled = false;
            Instance = this;

        }
    }
}
