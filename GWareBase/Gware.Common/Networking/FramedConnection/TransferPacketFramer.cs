using Gware.Common.DataStructures;
using Gware.Common.Data;
using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.FramedConnection
{
    public class TransferPacketFramer
    {
        private CircularBuffer<byte> m_buffer;
        private int m_requiredTotal;
        private bool m_building;
        private object m_lock = new object();

        public TransferPacketFramer()
        {
            m_buffer = new CircularBuffer<byte>((int)(TransferDataPacket.c_maxPacketDataSize*2.5));
            m_building = false;
        }
        public List<TransferDataPacket> AddBytes(byte[] bytes)
        {
            lock (m_lock)
            {
                m_buffer.Write(bytes);
                List<TransferDataPacket> retVal = new List<TransferDataPacket>();
                TransferDataPacket bufferPacket = null;

                do
                {
                    bufferPacket = CheckBufferForPacket();
                    if (bufferPacket != null)
                    {
                        retVal.Add(bufferPacket);
                    }
                } while (bufferPacket != null);

                return retVal;
            }
        }

        private TransferDataPacket CheckBufferForPacket()
        {
            TransferDataPacket retVal = null;

            if (!m_building)
            {
                if (m_buffer.UnreadCount >= TransferDataPacketHeader.SizeRequiredForDataLength)
                {
                    byte[] receviedHeader = m_buffer.Peek(TransferDataPacketHeader.SizeRequiredForDataLength);
                    ushort headerLength = BitConverter.ToUInt16(receviedHeader, TransferDataPacketHeader.HeaderLengthLocation);
                    ushort dataLength = BitConverter.ToUInt16(receviedHeader, TransferDataPacketHeader.DataLengthLocation);

                    m_requiredTotal = headerLength + dataLength;
                    m_building = true;
                }
            }

            if (m_building && m_buffer.UnreadCount >= m_requiredTotal)
            {
                byte[] packetBytes = m_buffer.Read(m_requiredTotal);

                retVal = new TransferDataPacket();
                retVal.FromBytes(packetBytes);

                m_building = false;
                m_requiredTotal = 0;
            }

            return retVal;
        }
    }
}
