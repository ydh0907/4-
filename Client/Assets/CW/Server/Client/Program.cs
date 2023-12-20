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
        public static List<Room> Rooms = new List<Room>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

        }
        private void Update()
        {
            if (connector is not null)
                connect = connector.onConnecting;
        }
        private IEnumerator ServerConnect()
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.3.130");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            serverSession = new ServerSession();
            connector = new Connector(endPoint, serverSession);
            connector.StartConnect(endPoint);

            yield return new WaitWhile(() => true);

        }

        public void DisConnectServer(Action testaction = null)
        {
            Debug.Log("DisConnect");
            serverSession.Close();
            StopCoroutine("ServerConnect");

            testaction?.Invoke();
        }

        [ContextMenu("Create")]
        public void Create()
        {
            StartCoroutine(CreateTest());
        }

        public IEnumerator CreateTest()
        {
            StartCoroutine("ServerConnect");

            yield return new WaitWhile(() => !connect);
            Debug.Log("connect");

            S_RoomCreatePacket packet = new S_RoomCreatePacket();
            packet.roomName = "a";
            packet.makerName = "b";
            packet.playerCount = 1;

            serverSession.Send(packet.Serialize());
        }

        [ContextMenu("Reload")]
        public void Reload()
        {
            StartCoroutine(ReLoadTest());
        }

        public IEnumerator ReLoadTest()
        {
            StartCoroutine("ServerConnect");

            S_ReRoadingPacket packet = new S_ReRoadingPacket();

            yield return new WaitWhile(() => !connect);
            Debug.Log("connect");

            serverSession.Send(packet.Serialize());
        }

        [ContextMenu("Room")]
        public List<Room> Roomtest()
        {
            Debug.Log(Rooms.Count);
            foreach (Room room in Rooms)
            {
                Debug.Log($" room :{room.roomName} maker :{room.makerName} pc :{room.playerCount}");
            }

            return Rooms;
        }


    }
}