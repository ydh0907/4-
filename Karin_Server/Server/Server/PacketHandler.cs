using Karin.Network;
using Packets;

namespace TestServer
{
    public class PacketHandler
    {
        public static void S_ReRoadingPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;

            C_ReRoadingPacket c_ReRoadingPacket = new C_ReRoadingPacket();
            c_ReRoadingPacket.Rooms = Program.Rooms;

            clientSession.Send(c_ReRoadingPacket.Serialize());
        }
        public static void S_RoomCreatePacket(Session session, Packet packet)
        {
            S_RoomCreatePacket s_RoomCreatePacket = packet as S_RoomCreatePacket;
            Room newRoom = new()
            {
                roomName = s_RoomCreatePacket.roomName,
                makerName = s_RoomCreatePacket.makerName,
                playerCount = s_RoomCreatePacket.playerCount
            };
            Program.Rooms.Add(newRoom);
            ClientSession clientSession = session as ClientSession;

            C_RoomCreatePacket c_RoomCreatePacket = new C_RoomCreatePacket();

            clientSession.Send(c_RoomCreatePacket.Serialize());
        }
    }
}
