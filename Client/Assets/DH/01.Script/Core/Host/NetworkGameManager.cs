using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DH
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static NetworkGameManager Instance = null;

        [SerializeField] private GameObject Player;
        public UnityEvent onGameStart = new();
        public UnityEvent onGameEnded = new();
        public UnityEvent<Dictionary<ulong, PlayerInfo>> onPlayerValueChanged = new();

        public PlayerDictionary<PlayerInfo> players = new();

        public override void OnNetworkSpawn()
        {
            Instance = this;

            base.OnNetworkSpawn();
            GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
            SetPlayerListClientRpc(players);

            StartCoroutine(Starting());
        }

        [ClientRpc]
        private void SetPlayerListClientRpc(PlayerDictionary<PlayerInfo> players)
        {
            this.players = players;
        }

        private IEnumerator Starting()
        {
            while (NetworkServerApprovalManager.Instance.isHandlingConnect) yield return null;
            NetworkServerApprovalManager.Instance.ApprovalShutdown = true;

            foreach (var info in players.GetDummy().Values)
            {
                GameObject player = Instantiate(Player, Vector3.zero, Quaternion.identity);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(info.ID);
            }

            onGameStart?.Invoke();
        }

        [ServerRpc]
        public void EndGameServerRpc()
        {
            onGameEnded?.Invoke();

            EndCallClientRpc();

            DisconnectAll();
        }

        [ClientRpc]
        private void EndCallClientRpc()
        {
            onGameEnded?.Invoke();
        }

        private void DisconnectAll()
        {
            //
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

    public class PlayerDictionary<Value>
    {
        private Dictionary<ulong, Value> players;

        public Action<Dictionary<ulong, Value>> onValueChanged = null;

        public int Count => players.Count;

        public void Add(ulong key, Value value)
        {
            players.Add(key, value);
            onValueChanged?.Invoke(players);
        }

        public void Remove(ulong key)
        {
            players.Remove(key);
            onValueChanged?.Invoke(players);
        }

        public void Set(ulong key, Value value)
        {
            players[key] = value;
            onValueChanged?.Invoke(players);
        }

        public Dictionary<ulong, Value> GetDummy()
        {
            Dictionary<ulong, Value> dummy = new();

            foreach(var player in players)
            {
                dummy[player.Key] = player.Value;
            }

            return dummy;
        }

        public bool ContainsKey(ulong key)
        {
            return players.ContainsKey(key);
        }
    }
}
