using Karin.Network;
using Packets;
using System;
using System.Collections.Generic;

namespace TestClient
{
    public class PacketManager
    {
        private static PacketManager instance;
        public static PacketManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PacketManager();
                }
                return instance;
            }
            set { }
        }

        private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories =
            new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>();

        private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>();

        public PacketManager()
        {
            packetFactories.Clear();
            packetHandlers.Clear();

            RegisterHandler();
        }

        private void RegisterHandler()
        {
            packetFactories.Add((ushort)PacketID.C_RoomCreatePacket, PacketUtillity.CreatePacket<C_RoomCreatePacket>);
            packetHandlers.Add((ushort)PacketID.C_RoomCreatePacket, PacketHandler.C_RoomCreatePacket);

            packetFactories.Add((ushort)PacketID.C_ReRoadingPacket, PacketUtillity.CreatePacket<C_ReRoadingPacket>);
            packetHandlers.Add((ushort)PacketID.C_ReRoadingPacket, PacketHandler.C_ReRoadingPacket);
        }

        public void HandlePacket(Session session, ArraySegment<byte> buffer)
        {
            // [ Size ] [ ID ] [ Data ] [ ... ]

            ushort packetID = PacketUtillity.ReadPacketID(buffer);

            if (packetFactories.ContainsKey(packetID))
            {
                Packet packet = packetFactories[packetID]?.Invoke(buffer);
                if (packetHandlers.ContainsKey(packetID))
                {
                    packetHandlers[packetID]?.Invoke(session, packet);
                }

            }

        }


    }
}
