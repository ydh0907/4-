using Karin.Network;
using Packets;
using System.Net;
using UnityEngine;

namespace TestClient
{
    internal class Program : MonoBehaviour
    {
        public static Connector connector;
        public static ServerSession serverSession;

        [ContextMenu("Server")]
        private async void ServerConnect()
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.3.130");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            serverSession = new ServerSession();
            connector = new Connector(endPoint, serverSession);
            connector.StartConnect(endPoint);

            //while (true)
            //{


            //}

        }

        public static async void CreateTest()
        {
            while (true)
            {

            }
        }

    }
}