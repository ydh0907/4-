using Karin.Network;
using Packets;
using System.Net;

namespace TestClient
{
    internal class Program
    {
        public static Connector connector;
        public static ServerSession serverSession;

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.3.130");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            serverSession = new ServerSession();
            connector = new Connector(endPoint, serverSession);
            connector.StartConnect(endPoint);

            while (true)
            {


            }

        }

        public static async void CreateTest()
        {
            while (true)
            {

            }
        }

    }
}