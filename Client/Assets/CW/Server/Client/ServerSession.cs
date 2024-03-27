using Karin.Network;
using Packets;
using System;
using System.Net;
using UnityEngine;

namespace TestClient
{
    public class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Program.messages.Enqueue($"[Session] Connected with Server");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Program.messages.Enqueue($"[Session] Disconnected with Server");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.HandlePacket(this, buffer);
        }

        public override void OnSent(int length)
        {
            Program.messages.Enqueue($"[Session] {length} of Data Sent");
        }
    }
}
