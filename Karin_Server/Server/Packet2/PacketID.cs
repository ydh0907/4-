namespace Packets
{
    public enum PacketID : ushort
    {
        C_ReRoadingPacket, //방정보 넘기기
        S_ReRoadingPacket, //방정보 넘기라고 요청 
        S_RoomCreatePacket, //방만들었다고 알리기
        C_RoomCreatePacket, //방만들어졌다고 알리는 패킷
    }
}
