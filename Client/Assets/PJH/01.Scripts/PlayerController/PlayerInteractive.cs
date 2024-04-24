using Unity.Netcode;
using UnityEngine;
using AH;

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
            if (mentosCount > 0)
            {
                if (!DamageCaster.isMentosMode)
                {
                    DamageCaster.EnableMentosAttack();
                    EnableMentosServerRpc();
                    mentosCount = 0;

                    IngameUIToolkit.instance.ChangeMantosAttack();
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
            if (IsOwner)
            {
                IngameUIToolkit.instance.ChangeFistAttack();
            }
            DamageCaster.DisableMentosMode();
            Debug.Log("Disable Mentos");
        }
    }
}
