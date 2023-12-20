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

        public void DisConnectServer(Action testaction = null)
        {
            serverSession.Close();

            testaction?.Invoke();
        }

        public void Create()
        {
            StartCoroutine(CreateTest());
        }

        public IEnumerator CreateTest()
        {
            StartCoroutine("ServerConnect");

            yield return new WaitUntil(() => connect);

            S_RoomCreatePacket packet = new S_RoomCreatePacket();
            packet.roomName = "a";
            packet.makerName = "b";
            packet.playerCount = 1;

            serverSession.Send(packet.Serialize());
        }

        public void Reload(Action<List<Room>> callback)
        {
            StartCoroutine(ReLoadTest(callback));
        }

        public IEnumerator ReLoadTest(Action<List<Room>> callback)
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