using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public enum Cola
    {
        Cola,
        Pineapple,
        Sprite,
        Orange
    }

    public enum Character
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

    public class PlayerInfo : INetworkSerializable
    {
        public ulong ID = 0;
        public string Nickname = "";
        public Cola Cola = Cola.Cola;
        public Character Char = Character.Beach;
        public bool Ready = false;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ID);
            serializer.SerializeValue(ref Nickname);
            serializer.SerializeValue(ref Cola);
            serializer.SerializeValue(ref Char);
            serializer.SerializeValue(ref Ready);
        }

        public PlayerInfo() { }

        public PlayerInfo(ulong ID, string Nickname, Cola Cola, Character Char)
        {
            this.ID = ID;
            this.Nickname = Nickname;
            this.Cola = Cola;
            this.Char = Char;

            if(this.Nickname == null) this.Nickname = "";
        }

        public PlayerInfo(string Nickname, Cola Cola, Character Char)
        {
            this.Nickname = Nickname;
            this.Cola = Cola;
            this.Char = Char;

            if (this.Nickname == null) this.Nickname = "";
        }

        public PlayerInfo(string Nickname, Cola Cola, Character Char, bool Ready)
        {
            this.Nickname = Nickname;
            this.Cola = Cola;
            this.Char = Char;
            this.Ready = Ready;

            if (this.Nickname == null) this.Nickname = "";
        }

        public PlayerInfo(ulong ID, string Nickname, Cola Cola, Character Char, bool Ready)
        {
            this.ID = ID;
            this.Nickname = Nickname;
            this.Cola = Cola;
            this.Char = Char;
            this.Ready = Ready;

            if (this.Nickname == null) this.Nickname = "";
        }
    }
}
