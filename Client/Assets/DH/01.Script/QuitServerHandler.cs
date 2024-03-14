using DH;
using Karin.Network;
using Packets;
using System.Net;
using System.Threading;
using TestClient;
using UnityEngine;

public class QuitServerHandler
{
    public static bool QuitDeletingFlag = false;

    public static void DeleteImmediately(string nickname, string IP)
    {
        DeletingAfterJobs(nickname, IP);
    }

    private static void DeletingAfterJobs(string nickname, string IP)
    {
        QuitDeletingFlag = true;
        IPEndPoint endPoint = new(IPAddress.Parse("116.33.174.234"), 31408);
        ServerSession session = new ServerSession();
        Connector connector = new Connector(endPoint, session);
        connector.StartConnect(endPoint);

        while (!connector.onConnecting) { }

        while (session.Active == 0) { }

        S_RoomDeletePacket s_RoomDeletePacket = new S_RoomDeletePacket();
        s_RoomDeletePacket.makerName = nickname;
        s_RoomDeletePacket.roomName = IP;
        session.Send(s_RoomDeletePacket.Serialize());

        while (QuitDeletingFlag && NetworkGameManager.MatchingServerConnection) { }

        Debug.Log("Room Deleted");
    }
}
