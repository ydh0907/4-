using Karin.Network;
using Packets;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    internal class Program
    {
        public static Listener listener;
        public static List<Room> Rooms = new();

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.3.130");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            listener = new Listener(endPoint);
            if (listener.Listen(10))
            {
                listener.StartAccept(onAccepted);
            }

            while(true)
            {

            }

        }

        private static void onAccepted(Socket socket)
        {
            ClientSession session = new ClientSession();
            session.Open(socket);
            session.OnConnected(socket.RemoteEndPoint);
        }

    }
}