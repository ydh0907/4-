using Karin.Network;
using Packets;
using System;

namespace Packets
{
    public class C_RoomDeletePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_RoomDeletePacket;



        public override void Deserialize(ArraySegment<byte> buffer)
        {
            // 역직렬화
            // [ 전체 길이 ] [ 패킷 ID ] [ 메세지 길이 ] [ 메세지 ]....

            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            //전체 길이와 패킷 ID 다음부터 읽기위해서

        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;
            process += sizeof(ushort);

            process += PacketUtillity.AppendUShortData(this.ID, buffer, process);
            PacketUtillity.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}

