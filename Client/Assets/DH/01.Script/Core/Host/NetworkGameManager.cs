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

        public NetworkVariable<PlayerDictionary<PlayerInfo>> players = new();

        public override void OnNetworkSpawn()
        {
            Instance = this;

            base.OnNetworkSpawn();
            GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(StartGame);
        }

        public void StartGame()
        {StartCoroutine(Starting());
        }

        private IEnumerator Starting()
        {
            while (NetworkServerApprovalManager.Instance.isHandlingConnect) yield return null;
            NetworkServerApprovalManager.Instance.ApprovalShutdown = true;

            foreach (var info in players.Value.GetDummy().Values)
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
            NetworkManager.Singleton.Shutdown();
        }
    }
}
