using Karin.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packets
{
    public class C_ReRoadingPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ReRoadingPacket;

        public List<Room> Rooms = new();

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);


            process += PacketUtillity.ReadListData<Room>(buffer, process, out this.Rooms);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;
            process += sizeof(ushort);

            process += PacketUtillity.AppendUShortData(this.ID, buffer, process);
            process += PacketUtillity.AppendListData<Room>(Rooms, buffer, process);
            PacketUtillity.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }

    }
}
