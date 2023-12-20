using Karin.Network;
using Packets;

namespace TestServer
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
            packetFactories.Add((ushort)PacketID.S_RoomCreatePacket, PacketUtillity.CreatePacket<S_RoomCreatePacket>);
            packetHandlers.Add((ushort)PacketID.S_RoomCreatePacket, PacketHandler.S_RoomCreatePacket);

            packetFactories.Add((ushort)PacketID.S_ReRoadingPacket, PacketUtillity.CreatePacket<S_ReRoadingPacket>);
            packetHandlers.Add((ushort)PacketID.S_ReRoadingPacket, PacketHandler.S_ReRoadingPacket);

            packetFactories.Add((ushort)PacketID.S_RoomDeletePacket, PacketUtillity.CreatePacket<S_RoomDeletePacket>);
            packetHandlers.Add((ushort)PacketID.S_RoomDeletePacket, PacketHandler.S_RoomDeletePacket);
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
