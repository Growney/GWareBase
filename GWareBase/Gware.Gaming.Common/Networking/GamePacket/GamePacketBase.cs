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
    public abstract class GamePacketBase : IGamePacket
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
        public long StopWatchTime { get; set; }
        public ushort PacketID { get; private set; }
        public int TypeID { get; private set; }
        
        protected GamePacketBase(ushort packetID)
        {
            TypeID = GetType().GetClassID<GamePacketAttribute>();
            PacketID = packetID;
        }

        public GamePacketBase()
            :this(GetNextID())
        {
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
            writer.WriteInt64(StopWatchTime);
            writer.WriteInt16(PacketID);
            OnToWriter(writer);
        }
        public void FromBuffer(BufferReader reader)
        {
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
        
    }
}
