using Karin.Network;
using System.Net;

namespace TestServer
{
    public class ClientSession : Session
    {
        public EndPoint EndPoint;

        public override void OnConnected(EndPoint endPoint)
        {
            this.EndPoint = endPoint;
            Console.WriteLine($"[Session] Client Connected : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client DisConnected : {endPoint}");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            Console.WriteLine($"[Session] {buffer.Count} of Data Rerceived");
            PacketManager.Instance.HandlePacket(this, buffer);
        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"[Session] {length} of Data Sent");
        }
    }
}
