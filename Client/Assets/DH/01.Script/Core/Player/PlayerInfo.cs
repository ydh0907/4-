using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DH
{
    public enum Cola
    {
        CocaCola,
        Sprite,
        DrPepper,
        Pepsi
    }

    public struct PlayerInfo
    {
        public ulong ID;
        public string Nickname;
        public Cola Cola;
        public int Kill;
        public int Death;

        public PlayerInfo(ulong ID, string Nickname)
        {
            this.ID = ID;
            this.Nickname = Nickname;
            Cola = Cola.CocaCola;
            Kill = 0;
            Death = 0;
        }

        public PlayerInfo(ulong ID, string Nickname, Cola Cola)
        {
            this.ID = ID;
            this.Nickname = Nickname;
            this.Cola = Cola;
            Kill = 0;
            Death = 0;
        }
    }
}
