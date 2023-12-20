using Karin.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packets
{
    public class Room : DataPacket
    {
        public string roomName;
        public string makerName;
        public ushort playerCount;

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtillity.ReadStringData(buffer, offset + process, out roomName);
            process += PacketUtillity.ReadStringData(buffer, offset + process, out makerName);
            process += PacketUtillity.ReadUShortData(buffer, offset + process, out playerCount);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;

            process += PacketUtillity.AppendStringData(roomName, buffer, offset + process);
            process += PacketUtillity.AppendStringData(makerName, buffer, offset + process);
            process += PacketUtillity.AppendUShortData(playerCount, buffer, offset + process);

            return process;
        }
    }
}
