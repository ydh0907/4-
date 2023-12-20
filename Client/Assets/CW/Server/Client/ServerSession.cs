using Karin.Network;
using Packets;
using System;
using System.Net;

namespace TestClient
{
    public class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Connected with Server");

            Program.CreateTest();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] DisConnected with Server");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            Console.WriteLine($"[Session] {buffer.Count} of Data Received");
            PacketManager.Instance.HandlePacket(this, buffer);
        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"[Session] {length} of Data Sent");
        }
    }
}
