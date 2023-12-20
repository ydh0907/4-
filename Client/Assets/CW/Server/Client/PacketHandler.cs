﻿using Karin.Network;
using Packets;
using UnityEngine;

namespace TestClient
{
    public class PacketHandler
    {
        public static void C_RoomCreatePacket(Session session, Packet packet)
        {
            C_RoomCreatePacket c_RoomCreatePacket = packet as C_RoomCreatePacket;
            Program.Instance.DisConnectServer();
        }

        public static void C_ReRoadingPacket(Session session, Packet packet)
        {
            C_ReRoadingPacket c_ReRoadingPacket = packet as C_ReRoadingPacket;
            Program.Rooms = c_ReRoadingPacket.Rooms;
            Program.Instance.reload = true;
            Program.Instance.DisConnectServer();
        }
    }
}