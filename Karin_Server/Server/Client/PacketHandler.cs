using Karin.Network;
using Packets;

namespace TestClient
{
    public class PacketHandler
    {
        public static void C_RoomCreatePacket(Session session, Packet packet)
        {
            C_RoomCreatePacket c_RoomCreatePacket = packet as C_RoomCreatePacket;
            // 방만들기 완료
        }

        public static void C_ReRoadingPacket(Session session, Packet packet)
        {
            C_ReRoadingPacket c_ReRoadingPacket = packet as C_ReRoadingPacket;
            Console.WriteLine(c_ReRoadingPacket.Rooms.Count);
            // 방정보 공유 .Rooms
        }
    }
}
