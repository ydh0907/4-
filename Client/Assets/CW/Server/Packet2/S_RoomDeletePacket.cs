using Karin.Network;
using System;

namespace Packets
{
    public class S_RoomDeletePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_RoomDeletePacket;

        public string roomName;
        public string makerName;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            // 역직렬화
            // [ 전체 길이 ] [ 패킷 ID ] [ 메세지 길이 ] [ 메세지 ]....

            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtillity.ReadStringData(buffer, process, out this.roomName);
            process += PacketUtillity.ReadStringData(buffer, process, out this.makerName);
            //전체 길이와 패킷 ID 다음부터 읽기위해서

        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;
            process += sizeof(ushort);

            process += PacketUtillity.AppendUShortData(this.ID, buffer, process);
            process += PacketUtillity.AppendStringData(this.roomName, buffer, process);
            process += PacketUtillity.AppendStringData(this.makerName, buffer, process);
            PacketUtillity.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}

