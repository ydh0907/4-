using DH;
using Karin.Network;
using Packets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace TestClient
{
    public class Program : MonoBehaviour
    {
        public static Connector connector;
        public static ServerSession serverSession;

        public static Program Instance;

        public bool connect = false;
        public bool reload = false;
        public List<Room> Rooms = new List<Room>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else Destroy(this);
        }

        private IEnumerator ServerConnect()
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.1.237");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            serverSession = new ServerSession();
            connector = new Connector(endPoint, serverSession);
            connector.StartConnect(endPoint);

            yield return new WaitUntil(() =>
            {
                connect = connector.onConnecting;
                return connect;
            });
        }

        public void DisconnectServer()
        {
            serverSession.Close();
            connect = false;
        }

        public void CreateRoom(string IP, string playerName)
        {
            Debug.Log($"Create Room {IP} : {playerName}");
            StartCoroutine(SendRoomUpdatePacket(IP, playerName, 1));
        }

        public void UpdateRoom(string IP, string playerName, int playerCount)
        {
            Debug.Log($"Update Room {IP} : {playerName}");
            StartCoroutine(SendRoomUpdatePacket(IP, playerName, (ushort)playerCount));
        }

        private IEnumerator SendRoomUpdatePacket(string IP, string playerName, ushort playerCount)
        {
            StartCoroutine("ServerConnect");

            yield return new WaitUntil(() => connect);

            S_RoomCreatePacket packet = new S_RoomCreatePacket();
            packet.roomName = IP;
            packet.makerName = playerName;
            packet.playerCount = playerCount;

            serverSession.Send(packet.Serialize());
        }

        public void Reload(Action<List<Room>> callback)
        {
            StartCoroutine(Reloading(callback));
        }

        private IEnumerator Reloading(Action<List<Room>> callback)
        {
            StartCoroutine("ServerConnect");

            yield return new WaitUntil(() => connect);

            S_ReRoadingPacket packet = new S_ReRoadingPacket();
            serverSession.Send(packet.Serialize());

            Debug.Log("Reroad Send");

            yield return new WaitUntil(() => reload);

            reload = false;

            callback?.Invoke(Rooms);
            Debug.Log("Reroaded");
        }

        public void Delete(string nickname, string IP)
        {
            Debug.Log($"Delete Room {IP} : {nickname}");
            StartCoroutine(Deleting(nickname, IP));
        }

        private IEnumerator Deleting(string nickname, string IP)
        {
            StartCoroutine("ServerConnect");

            yield return new WaitUntil(() => connect);

            S_RoomDeletePacket s_RoomDeletePacket = new();
            s_RoomDeletePacket.makerName = nickname;
            s_RoomDeletePacket.roomName = IP;

            serverSession.Send(s_RoomDeletePacket.Serialize());
        }
    }
}