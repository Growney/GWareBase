using Gware.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Packet
{
    public class TransferDataPacket
    {
        public const int c_maxPacketDataSize = 8192;

        private TransferDataPacketHeader m_header = new TransferDataPacketHeader();

        public TransferDataPacketHeader Header
        {
            get { return m_header; }
            set { m_header = value; }
        }

        private byte[] m_data;

        public byte[] Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public TransferDataPacket()
        {

        }

        public void FromBytes(byte[] bytes)
        {
            m_header.FromBytes(bytes);

            BufferReader reader = new BufferReader(bytes);
            m_data = reader.ReadBytesAt(m_header.HeaderLength, m_header.DataLength);
        }

        public byte[] ToBytes()
        {
            BufferWriter preCRCWriter = new BufferWriter();
            if (m_data != null)
            {
                m_header.DataLength = (ushort)m_data.Length;
            }
            preCRCWriter.WriteBytes(m_header.ToBytes());
            preCRCWriter.WriteBytes(m_data);

            byte[] bufferPreCRC = preCRCWriter.GetBuffer();

            uint crc = 0;
            GenerateCRC.GenerateCRC32(bufferPreCRC, bufferPreCRC.Length,ref crc);

            m_header.PacketCRC = crc;

            BufferWriter postCRCWriter = new BufferWriter();
            postCRCWriter.WriteBytes(m_header.ToBytes());
            postCRCWriter.WriteBytes(m_data);

            return postCRCWriter.GetBuffer();
        }

        public static List<TransferDataPacket> GetPackets(byte[] data)
        {
            uint crc = 0;
            GenerateCRC.GenerateCRC32(data, data.Length, ref crc);

            ushort totalPackets = (ushort)(data.Length/c_maxPacketDataSize);
            if(data.Length%c_maxPacketDataSize > 0)
            {
                totalPackets++;
            }

            List<TransferDataPacket> retVal = new List<TransferDataPacket>();
            BufferReader reader = new BufferReader(data);
            int readBytes = 0;
            for (int bytesToRead = Math.Min(data.Length, c_maxPacketDataSize); retVal.Count < totalPackets; bytesToRead = Math.Min(data.Length - readBytes, c_maxPacketDataSize))
            {
                TransferDataPacket newPacket = new TransferDataPacket();
                newPacket.Header.DataCRC = crc;
                newPacket.Header.PacketTotal = totalPackets;
                newPacket.Header.PacketNumber = (ushort)retVal.Count;
                newPacket.Data = reader.ReadBytes(bytesToRead);
                readBytes += bytesToRead;
                retVal.Add(newPacket);

            }
            return retVal;
        }

        public static byte[] GetData(IEnumerable<TransferDataPacket> packets)
        {
            BufferWriter writer = new BufferWriter();
            foreach(TransferDataPacket dp in packets)
            {
                writer.WriteBytes(dp.Data);
            }
            return writer.GetBuffer();
        }
    }
}
