using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Data
{
    public class BufferReader
    {
        #region Declarations

        protected byte[] m_ByteBuffer;
        protected int m_CurrentPos;
        protected bool m_bUseNetworkByteOrder;

        #endregion Declarations

        #region Construction

        public BufferReader()
            : this(Application.ApplicationBase.c_useNetworkOrder)
        {

        }
        public BufferReader(byte[] buff)
            :this(buff,Application.ApplicationBase.c_useNetworkOrder)
        {

        }
        public BufferReader(bool bUseNetworkByteOrder)
        {
            m_CurrentPos = 0;
            m_bUseNetworkByteOrder = bUseNetworkByteOrder;
            m_ByteBuffer = null;
        }

        public BufferReader(byte[] buff, bool bUseNetworkByteOrder)
        {
            m_CurrentPos = 0;
            m_bUseNetworkByteOrder = bUseNetworkByteOrder;
            m_ByteBuffer = buff;
        }

        #endregion Construction

        #region Properties

        public int Position
        {
            get { return (m_CurrentPos); }
            set
            {
                if (value < 1)
                    m_CurrentPos = 0;
                else if (value > m_ByteBuffer.Length)
                    m_CurrentPos = m_ByteBuffer.Length - 1;
                else
                    m_CurrentPos = value;
            }
        }

        public int BufferLength
        {
            get { return (m_ByteBuffer != null ? m_ByteBuffer.Length : 0); }
        }

        public bool UseNetworkByteOrder
        {
            get { return (m_bUseNetworkByteOrder); }
            set { m_bUseNetworkByteOrder = value; }
        }

        #endregion Properties

        #region Methods

        public byte[] GetBuffer()
        {
            return (m_ByteBuffer);
        }

        public byte[] GetBuffer(int Length)
        {
            if (Length < 1 || Length >= m_ByteBuffer.Length)
                return (m_ByteBuffer);
            else
            {
                byte[] buff = new byte[Length];
                Buffer.BlockCopy(m_ByteBuffer, 0, buff, 0, Length);
                return (buff);
            }
        }

        public void SetBuffer(byte[] Buff)
        {
            SetBuffer(Buff, false);
        }

        public void SetBuffer(byte[] Buff, bool Copy)
        {
            m_CurrentPos = 0;
            if (!Copy)
                m_ByteBuffer = Buff;
            else
            {
                m_ByteBuffer = (byte[])Buff.Clone();
            }
        }

        #endregion Methods

        #region CheckBuffer

        public bool CheckBuffer(int size)
        {
            return (CheckBuffer(m_CurrentPos, size));
        }

        public bool CheckBuffer(int Index, int size)
        {
            return ((m_ByteBuffer != null) && (size > 0) && (m_ByteBuffer.Length > (Index + (size - 1))));
        }

        #endregion CheckBuffer

        #region Reader Methods

        public ulong ReadUInt64()
        {
            return ((ulong)ReadInt64());
        }

        public long ReadInt64()
        {
            long ret = ReadInt64At(m_CurrentPos);

            m_CurrentPos += sizeof(long);

            return (ret);
        }

        public uint ReadUInt32()
        {
            return ((uint)ReadInt32());
        }

        public int ReadInt32()
        {
            int ret = ReadInt32At(m_CurrentPos);

            m_CurrentPos += sizeof(int);

            return (ret);
        }

        public ushort ReadUInt16()
        {
            return ((ushort)ReadInt16());
        }

        public short ReadInt16()
        {
            short ret = ReadInt16At(m_CurrentPos);

            m_CurrentPos += sizeof(short);

            return (ret);
        }

        public byte ReadByte()
        {
            byte ret = ReadByteAt(m_CurrentPos);

            m_CurrentPos++;

            return (ret);
        }

        public long[] ReadInt64s(int Count)
        {
            long[] ret = ReadInt64sAt(m_CurrentPos, Count);

            m_CurrentPos += (Count * sizeof(long));

            return (ret);
        }

        public int[] ReadInt32s(int Count)
        {
            int[] ret = ReadInt32sAt(m_CurrentPos, Count);

            m_CurrentPos += (Count * sizeof(int));

            return (ret);
        }

        public short[] ReadInt16s(int Count)
        {
            short[] ret = ReadInt16sAt(m_CurrentPos, Count);

            m_CurrentPos += (Count * sizeof(short));

            return (ret);
        }

        public byte[] ReadBytes(int Count)
        {
            byte[] ret = ReadBytesAt(m_CurrentPos, Count);

            m_CurrentPos += (Count * sizeof(byte));

            return (ret);
        }
        public string ReadString()
        {
            int stringBytesLength = ReadInt32();
            byte[] stringBytes = ReadBytes(stringBytesLength);
            if (stringBytesLength > 0)
            {
                return Application.ApplicationBase.c_ApplicationEncoding.GetString(stringBytes);
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion Reader Methods

        #region Reader At Methods

       

        public long ReadInt64At(int Index)
        {
            if (CheckBuffer(Index, sizeof(long)))
                return (NetworkToHostOrder(BitConverter.ToInt64(m_ByteBuffer, Index)));

            return (0);
        }

        public int ReadInt32At(int Index)
        {
            if (CheckBuffer(Index, sizeof(int)))
                return (NetworkToHostOrder(BitConverter.ToInt32(m_ByteBuffer, Index)));

            return (0);
        }

        public short ReadInt16At(int Index)
        {
            if (CheckBuffer(Index, sizeof(short)))
                return (NetworkToHostOrder(BitConverter.ToInt16(m_ByteBuffer, Index)));

            return (0);
        }

        public byte ReadByteAt(int Index)
        {
            if ((m_ByteBuffer != null) && (m_ByteBuffer.Length > Index))
                return (m_ByteBuffer[Index]);

            return (0);
        }

        public long[] ReadInt64sAt(int Index, int Count)
        {
            int ByteCount = (Count * sizeof(long));

            if (CheckBuffer(Index, ByteCount))
            {
                long[] buff = new long[Count];

                Buffer.BlockCopy(m_ByteBuffer, Index, buff, 0, ByteCount);

                for (int i = 0; i < Count; i++)
                    buff[i] = NetworkToHostOrder(buff[i]);

                return (buff);
            }

            return (null);
        }

        public int[] ReadInt32sAt(int Index, int Count)
        {
            int ByteCount = (Count * sizeof(uint));
            if (CheckBuffer(Index, ByteCount))
            {
                int[] buff = new int[Count];

                Buffer.BlockCopy(m_ByteBuffer, Index, buff, 0, ByteCount);

                for (int i = 0; i < Count; i++)
                    buff[i] = NetworkToHostOrder(buff[i]);

                return (buff);
            }

            return (null);
        }

        public short[] ReadInt16sAt(int Index, int Count)
        {
            int ByteCount = (Count * sizeof(short));
            if (CheckBuffer(Index, ByteCount))
            {
                short[] buff = new short[Count];

                Buffer.BlockCopy(m_ByteBuffer, Index, buff, 0, ByteCount);

                for (int i = 0; i < Count; i++)
                    buff[i] = NetworkToHostOrder(buff[i]);

                return (buff);
            }

            return (null);
        }

        public byte[] ReadBytesAt(int Index, int Count)
        {
            if (CheckBuffer(Index, Count))
            {
                byte[] buff = new byte[Count];

                Buffer.BlockCopy(m_ByteBuffer, Index, buff, 0, Count);

                return (buff);
            }

            return (null);
        }

        #endregion Reader At Methods

        #region NetworkToHostOrder

        private short NetworkToHostOrder(short Item)
        {
            return (m_bUseNetworkByteOrder ? IPAddress.NetworkToHostOrder(Item) : Item);
        }

        private int NetworkToHostOrder(int Item)
        {
            return (m_bUseNetworkByteOrder ? IPAddress.NetworkToHostOrder(Item) : Item);
        }

        private long NetworkToHostOrder(long Item)
        {
            return (m_bUseNetworkByteOrder ? IPAddress.NetworkToHostOrder(Item) : Item);
        }

        #endregion HostToNetworkOrder
    }
}
