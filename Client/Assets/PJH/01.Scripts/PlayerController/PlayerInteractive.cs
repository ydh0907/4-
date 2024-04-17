using Unity.Netcode;
using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        public int mentosCount { get; private set; }

        [ClientRpc]
        public void AddMentosClientRpc()
        {
            mentosCount++;
        }

        public void UseMentos()
        {
            if (!IsOwner) return;
            if (DamageCaster.isMentosMode) return;

            if (mentosCount > 0)
            {
                if (!DamageCaster.isMentosMode)
                {
                    DamageCaster.EnableMentosAttack();
                    EnableMentosServerRpc();
                    mentosCount = 0;
                }
            }
        }

        [ServerRpc]
        public void EnableMentosServerRpc()
        {
            DamageCaster.EnableMentosAttack();
            EnableMentosClientRpc();
        }

        [ClientRpc]
        public void EnableMentosClientRpc()
        {
            if (IsOwner) return;
            DamageCaster.EnableMentosAttack();
        }

        [ClientRpc]
        public void DisableMentosClientRpc()
        {
            DamageCaster.DisableMentosMode();
            Debug.Log("Disable Mentos");
        }
    }
}
