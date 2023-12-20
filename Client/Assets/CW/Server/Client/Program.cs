using Karin.Network;
using Packets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace TestClient
{
    internal class Program : MonoBehaviour
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
            IPAddress ipAddress = IPAddress.Parse("172.31.3.130");
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

        public void DisConnectServer()
        {
            serverSession.Close();
        }

        public void Create(string roomName, string playerName)
        {
            StartCoroutine(SendRoomUpdatePacket(roomName, playerName, 1));
        }

        public void UpdateRoom(string roomName, string playerName, int playerCount)
        {
            StartCoroutine(SendRoomUpdatePacket(roomName, playerName, (ushort)playerCount));
        }

        public IEnumerator SendRoomUpdatePacket(string roomName, string playerName, ushort playerCount)
        {
            StartCoroutine("ServerConnect");

            yield return new WaitUntil(() => connect);

            S_RoomCreatePacket packet = new S_RoomCreatePacket();
            packet.roomName = roomName;
            packet.makerName = playerName;
            packet.playerCount = playerCount;

            serverSession.Send(packet.Serialize());
        }

        public void Reload(Action<List<Room>> callback)
        {
            StartCoroutine(Reloading(callback));
        }

        public IEnumerator Reloading(Action<List<Room>> callback)
        {
            StartCoroutine("ServerConnect");

            yield return new WaitUntil(() => connect);

            S_ReRoadingPacket packet = new S_ReRoadingPacket();
            serverSession.Send(packet.Serialize());

            yield return new WaitUntil(() => reload);

            reload = false;

            callback?.Invoke(Rooms);
        }
    }
}