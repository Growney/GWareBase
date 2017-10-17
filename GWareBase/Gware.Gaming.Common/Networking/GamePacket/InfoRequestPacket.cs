using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Packet;
using Gware.Common.Data;
using Gware.Common.Reflection;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public enum InfoRequestPacketType : byte
    {
        Ping,
    }
    [GamePacketAttribute(-1)]
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
        public int TypeID { get; private set; }

        public byte PacketTypePrefix
        {
            get
            {
                return 0;
            }
        }
        public InfoRequestPacket()
        {

            TypeID = GetType().GetClassID<GamePacketAttribute>();
        }
        public InfoRequestPacket(InfoRequestPacketType type)
            :this(type,GetNextID())
        {

        }

        protected InfoRequestPacket(InfoRequestPacketType type,ushort packetID)
            :this()
        {
            RequestType = type;
            PacketID = packetID;
        }

        public byte[] ToBytes()
        {
            BufferWriter writer = new BufferWriter(false);
            ToBuffer(writer);
            return writer.GetBuffer();
        }

        public void ToBuffer(BufferWriter writer)
        {
            writer.WriteInt32(TypeID);
            writer.WriteByte((byte)RequestType);
            writer.WriteInt64(StopWatchTime);
            writer.WriteInt16(PacketID);
            OnToWriter(writer);
        }

        public void FromBuffer(BufferReader reader)
        {
            RequestType = (InfoRequestPacketType)reader.ReadByte();
            StopWatchTime = reader.ReadInt64();
            PacketID = reader.ReadUInt16();
            OnFromReader(reader);
        }
        public virtual void OnFromReader(BufferReader reader)
        {

        }

        public virtual void OnToWriter(BufferWriter reader)
        {

        }

        public virtual IGamePacket CreateResponse()
        {
            InfoResponsePacket packet = new InfoResponsePacket();
            packet.PacketID = PacketID;
            packet.StopWatchTime = StopWatchTime;
            packet.RequestType = RequestType;
            return packet;
        }
    }
}
