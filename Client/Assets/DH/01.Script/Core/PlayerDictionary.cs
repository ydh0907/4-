using System;
using System.Collections.Generic;

namespace DH
{
    public class PlayerDictionary<Value>
    {
        private Dictionary<ulong, Value> players = new();
        public List<int> list {  get; private set; }
        public Action<Dictionary<ulong, Value>> onValueChanged = null;
        
        public bool IsShutdown { get; private set; }

        public int Count => players.Count;

        public void Add(ulong key, Value value)
        {
            if(IsShutdown) return;

            players.Add(key, value);
            onValueChanged?.Invoke(GetDummy());
        }

        public void Remove(ulong key)
        {
            if (IsShutdown) return;

            players.Remove(key);
            onValueChanged?.Invoke(GetDummy());
        }

        public void Set(ulong key, Value value)
        {
            if (IsShutdown) return;

            players[key] = value;
            onValueChanged?.Invoke(GetDummy());
        }

        public Dictionary<ulong, Value> GetDummy() // 여기서 얻는건 내용이 같은 다른 배열
        {
            Dictionary<ulong, Value> dummy = new();

            foreach (var player in players)
            {
                dummy[player.Key] = player.Value;
            }

            return dummy;
        }

        public bool ContainsKey(ulong key)
        {
            return players.ContainsKey(key);
        }

        public PlayerDictionary()
        {
            IsShutdown = false;
        }
    }

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
