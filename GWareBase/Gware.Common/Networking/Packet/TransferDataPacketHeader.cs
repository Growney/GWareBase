﻿using Gware.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Packet
{
    public class TransferDataPacketHeader
    {
        private bool m_useNetworkOrder;
        private ushort m_headerLength;
        private ushort m_packetNumber;
        private ushort m_packetTotal;
        private ushort m_dataLength;

        private int m_sequence;
        private uint m_ack;

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

        public uint Ack
        {
            get { return m_ack; }
            set { m_ack = value; }
        }

        public int Sequence
        {
            get { return m_sequence; }
            set { m_sequence = value; }
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

        public void FromBuffer(BufferReader reader)
        {
            HeaderLength = reader.ReadUInt16();//The header length and the data length must remain at the start of the packet header
            DataLength = reader.ReadUInt16();
            PacketNumber = reader.ReadUInt16();
            PacketTotal = reader.ReadUInt16();

            Ack = reader.ReadUInt32();
            Sequence = reader.ReadInt32();

            PacketCRC = reader.ReadUInt32();
            DataCRC = reader.ReadUInt32();
        }

        public void FromBytes(byte[] bytes)
        {
            BufferReader reader = new BufferReader(bytes);

            FromBuffer(reader);
        }

        public void ToBuffer(BufferWriter writer)
        {
            writer.WriteInt16(0);//The header length and the data length must remain at the start of the packet header
            writer.WriteInt16(DataLength);
            writer.WriteInt16(PacketNumber);
            writer.WriteInt16(PacketTotal);

            writer.WriteInt32((int)Ack);
            writer.WriteInt32(Sequence);

            writer.WriteInt32((int)PacketCRC);
            writer.WriteInt32((int)DataCRC);

            byte[] retVal = writer.GetBuffer();
            writer.WriteInt16At((short)retVal.Length, 0);
        }

        public byte[] ToBytes()
        {
            BufferWriter writer = new BufferWriter(m_useNetworkOrder);

            ToBuffer(writer);

            return writer.GetBuffer();
        }
    }
}
