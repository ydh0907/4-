using Unity.Netcode;
using UnityEngine;
using AH;
using System;

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
                    EnableMentosServerRpc();
                    mentosCount = 0;

                    IngameUIToolkit.instance.ChangeMantosAttack();
                }
            }
            else if (DamageCaster.isMentosMode)
            {
                Debug.Log($"Mentos Count : {mentosCount}, on disable mentos");
                DisableMentosServerRpc();
                mentosCount++;

                IngameUIToolkit.instance.ChangeFistAttack();
            }
        }

        [ServerRpc]
        private void DisableMentosServerRpc()
        {
            DamageCaster.DisableMentosMode();
            DisableMentosClientRpc();
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
        }
    }
}
