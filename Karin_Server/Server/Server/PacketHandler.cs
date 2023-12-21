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
            Console.WriteLine($"방개수 {Program.Rooms.Count}");
            c_ReRoadingPacket.Rooms = Program.Rooms;

            clientSession.Send(c_ReRoadingPacket.Serialize());
        }
        public static void S_RoomCreatePacket(Session session, Packet packet)
        {
            S_RoomCreatePacket s_RoomCreatePacket = packet as S_RoomCreatePacket;
            C_RoomCreatePacket c_RoomCreatePacket = new C_RoomCreatePacket();
            ClientSession clientSession = session as ClientSession;

            Room newRoom = new()
            {
                roomName = s_RoomCreatePacket.roomName,
                makerName = s_RoomCreatePacket.makerName,
                playerCount = s_RoomCreatePacket.playerCount
            };

            int i = 0;
            foreach (var t in Program.Rooms)
            {
                if (t.roomName == s_RoomCreatePacket.roomName && t.makerName == s_RoomCreatePacket.makerName)
                {
                    Program.Rooms[i].playerCount = s_RoomCreatePacket.playerCount;

                    Console.WriteLine($"방수정 {Program.Rooms.Count} room : {s_RoomCreatePacket.roomName} maker : {s_RoomCreatePacket.makerName} pc : {s_RoomCreatePacket.playerCount}");

                    clientSession.Send(c_RoomCreatePacket.Serialize());
                    return;
                }
                ++i;
            };

            Program.Rooms.Add(newRoom);
            Console.WriteLine($"방만듬 {Program.Rooms.Count} room : {s_RoomCreatePacket.roomName} maker : {s_RoomCreatePacket.makerName} pc : {s_RoomCreatePacket.playerCount}");

            clientSession.Send(c_RoomCreatePacket.Serialize());
        }

        public static void S_RoomDeletePacket(Session session, Packet packet)
        {
            S_RoomDeletePacket s_RoomDeletePacket = packet as S_RoomDeletePacket;
            ClientSession clientSession = session as ClientSession;

            int i = 0;
            foreach (var t in Program.Rooms)
            {
                if (t.roomName == s_RoomDeletePacket.roomName && t.makerName == s_RoomDeletePacket.makerName)
                {
                    Program.Rooms.RemoveAt(i);
                    break;
                }
                ++i;
            };

            C_RoomDeletePacket c_RoomDeletePacket = new C_RoomDeletePacket();


            clientSession.Send(c_RoomDeletePacket.Serialize());
        }
    }
}
