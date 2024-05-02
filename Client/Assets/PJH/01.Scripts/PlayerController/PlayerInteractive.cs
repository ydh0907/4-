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
            if (DamageCaster.isMentosMode)
            {
                Debug.Log($"Mentos Count : {mentosCount}, on disable mentos");
                DisableMentosServerRpc();
                mentosCount = 1;

                IngameUIToolkit.instance.ChangeFistAttack();
            }
            else if (mentosCount > 0)
            {
                if (!DamageCaster.isMentosMode)
                {
                    EnableMentosServerRpc();
                    mentosCount = 0;

                    IngameUIToolkit.instance.ChangeMantosAttack();
                }
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
