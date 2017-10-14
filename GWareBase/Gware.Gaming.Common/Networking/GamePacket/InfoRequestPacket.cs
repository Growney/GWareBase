using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Packet;
using Gware.Common.Data;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public enum InfoRequestPacketType : byte
    {
        Ping,
    }
    public class InfoRequestPacket : IGamePacket
    {
        private static object m_idLock = new object();
        private static ushort m_currentID = 0;

        private static ushort GetNextID()
        {
            lock (m_idLock)
            {
                return unchecked((ushort)(m_currentID + 1));
            }
        }

        public InfoRequestPacketType RequestType { get; private set; }
        public long StopWatchTime { get; set; }
        public ushort PacketID { get; private set; }

        public byte PacketTypePrefix
        {
            get
            {
                return 0;
            }
        }
        public InfoRequestPacket()
        {

        }
        public InfoRequestPacket(InfoRequestPacketType type)
            :this(type,GetNextID())
        {

        }

        protected InfoRequestPacket(InfoRequestPacketType type,ushort packetID)
        {
            RequestType = type;
            PacketID = packetID;
        }

        public void FromBytes(byte[] packet)
        {
            BufferReader reader = new BufferReader(packet);
            RequestType = (InfoRequestPacketType)reader.ReadByte();
            StopWatchTime = reader.ReadInt64();
            PacketID = reader.ReadUInt16();
        }

        public byte[] ToBytes()
        {
            BufferWriter writer = new BufferWriter(false);
            writer.WriteByte((byte)RequestType);
            writer.WriteInt64(StopWatchTime);
            writer.WriteInt16(PacketID);
            return writer.GetBuffer();
        }
    }
}
