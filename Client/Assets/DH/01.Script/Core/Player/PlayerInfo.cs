using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    public enum Char
    {
        Football,
        Beach,
        Business,
        Disco,
        Farmer,
        Police,
        Soccer,
        Thief,
    }

    public class PlayerInfo
    {
        public ulong ID = 0;
        public string Nickname = "";
        public Cola Cola = Cola.CocaCola;
        public int Kill = 0;
        public int Death = 0;

        public PlayerInfo() { }

        public PlayerInfo(ulong ID, string Nickname, Cola Cola)
        {
            this.ID = ID;
            this.Nickname = Nickname;
            this.Cola = Cola;

            if(this.Nickname == null) this.Nickname = "";
        }

        public PlayerInfo(string Nickname, Cola Cola)
        {
            this.Nickname = Nickname;
            this.Cola = Cola;

            if (this.Nickname == null) this.Nickname = "";
        }
    }
}
