using Gware.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Packet
{
    public class TransferDataPacketHeader
    {

        public static readonly int SizeRequiredForDataLength = sizeof(ushort) + sizeof(ushort);
        public static readonly int DataLengthLocation = sizeof(ushort);
        public static readonly int HeaderLengthLocation = 0;

        private bool m_useNetworkOrder;
        private ushort m_headerLength;
        private ushort m_packetNumber;
        private ushort m_packetTotal;
        private ushort m_dataLength;

        private long m_dateTimeTicks;

        private uint m_dataCRC;
        private uint m_packetCRC;

        public uint PacketCRC
        {
            get { return m_packetCRC; }
            set { m_packetCRC = value; }
        }
        public uint DataCRC
        {
            get { return m_dataCRC; }
            set { m_dataCRC = value; }
        }

        public DateTime DateTimeObject
        {
            get 
            { 
                return new DateTime(m_dateTimeTicks, DateTimeKind.Utc); 
            }
            set
            {   
                m_dateTimeTicks = value.ToUniversalTime().Ticks;
            }
        }

        public long DateTimeTicks
        {
            get { return m_dateTimeTicks; }
            set { m_dateTimeTicks = value; }
        }

        public ushort DataLength
        {
            get { return m_dataLength; }
            set { m_dataLength = value; }
        }
        public ushort PacketTotal
        {
            get { return m_packetTotal; }
            set { m_packetTotal = value; }
        }
        public ushort PacketNumber
        {
            get { return m_packetNumber; }
            set { m_packetNumber = value; }
        }
        public ushort HeaderLength
        {
            get { return m_headerLength; }
            set { m_headerLength = value; }
        }
        public TransferDataPacketHeader()
            :this(false)
        {

        }
        public TransferDataPacketHeader(bool useNetworkOrder)
        {
            m_useNetworkOrder = useNetworkOrder;
        }

        public void FromBytes(byte[] bytes)
        {
            BufferReader reader = new BufferReader(bytes);

            HeaderLength = (ushort)reader.ReadInt16();//The header length and the data length must remain at the start of the packet header
            DataLength = (ushort)reader.ReadInt16();
            PacketNumber = (ushort)reader.ReadInt16();
            PacketTotal = (ushort)reader.ReadInt16();

            DateTimeTicks = reader.ReadInt64();

            PacketCRC = (uint)reader.ReadInt32();
            DataCRC = (uint)reader.ReadInt32();

        }

        public byte[] ToBytes()
        {
            BufferWriter writer = new BufferWriter(m_useNetworkOrder);

            writer.WriteInt16(0);//The header length and the data length must remain at the start of the packet header
            writer.WriteInt16(DataLength);
            writer.WriteInt16(PacketNumber);
            writer.WriteInt16(PacketTotal);

            writer.WriteInt64(DateTimeTicks);

            writer.WriteInt32((int)PacketCRC);
            writer.WriteInt32((int)DataCRC);

            byte[] retVal = writer.GetBuffer();
            writer.WriteInt16At((short)retVal.Length,0);

            return writer.GetBuffer();
        }
    }
}
