using Karin.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packets
{
    public class S_ReRoadingPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ReRoadingPacket;


        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);


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
