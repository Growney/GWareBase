using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Data
{
    public class BufferWriter
    {
        #region static

        public static int GetStringByteLen(string str)
        {
            return (ASCIIEncoding.ASCII.GetByteCount(str));
        }

        #endregion static

        #region Declarations

        protected byte[] m_ByteBuffer;
        protected int m_CurrentPos;
        protected bool m_bUseNetworkByteOrder;

        #endregion Declarations

        #region Construction
        public BufferWriter(bool bUseNetworkByteOrder)
        {
            Init(bUseNetworkByteOrder);
        }

        public BufferWriter(int BufferLength, bool bUseNetworkByteOrder)
        {
            Init(bUseNetworkByteOrder);
            m_ByteBuffer = new byte[BufferLength];
        }

        public BufferWriter(byte[] buff, bool bUseNetworkByteOrder)
        {
            Init(bUseNetworkByteOrder);
            m_ByteBuffer = buff;
        }

        private void Init(bool bUseNetworkByteOrder)
        {
            m_bUseNetworkByteOrder = bUseNetworkByteOrder;
            m_CurrentPos = 0;
            m_ByteBuffer = new byte[1];
        }

        #endregion Construction

        #region Properties

        public int BufferLength
        {
            get { return (m_ByteBuffer != null ? m_ByteBuffer.Length : 0); }
        }

        public int Position
        {
            get { return (m_CurrentPos); }
            set
            {
                if (value < 1 || m_ByteBuffer == null)
                    m_CurrentPos = 0;
                else if (value > m_ByteBuffer.Length)
                    m_CurrentPos = m_ByteBuffer.Length - 1;
                else
                    m_CurrentPos = value;
            }
        }

        public bool UseNetworkByteOrder
        {
            get { return (m_bUseNetworkByteOrder); }
            set { m_bUseNetworkByteOrder = value; }
        }

        #endregion Properties

        #region Methods

        public void Clear()
        {
            m_CurrentPos = 0;
            if (m_ByteBuffer != null)
                Array.Clear(m_ByteBuffer, 0, m_ByteBuffer.Length);
        }

        public byte[] GetData(int len)
        {
            return (GetData(0, len));
        }

        public byte[] GetData(int start, int len)
        {
            byte[] data = null;

            if (len > 0)
                data = new byte[len];

            CopyData(data, start, len);

            return (data);
        }

        public bool CopyData(byte[] buff, int start, int len)
        {
            if ((buff != null) && (len > 0) && (buff.Length >= len))
            {
                if (start < 0)
                    start = 0;

                if (m_ByteBuffer.Length > start)
                {
                    int x = (m_ByteBuffer.Length - start);
                    if (x >= len)
                        x = len;
                    else
                        Array.Clear(buff, x, len);

                    Buffer.BlockCopy(m_ByteBuffer, start, buff, 0, x);

                    return (true);
                }

                if (buff.Length > 0)
                    Array.Clear(buff, 0, buff.Length);
            }

            return (false);
        }

        public byte[] GetBuffer()
        {
            return (m_ByteBuffer);
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

        public bool IsSpace(int length)
        {
            return ((m_CurrentPos + length) < (m_ByteBuffer.Length + 1));
        }

        #endregion Methods

        #region Writer Methods

        public void WriteByte(byte Item)
        {
            WriteToBufferPos(new byte[] {Item},0,sizeof(byte));
        }

        public void WriteString(string Item,Encoding encoding)
        {
            if (Item == null)
            {
                Item = string.Empty;
            }
            byte[] stringBytes = encoding.GetBytes(Item);
            WriteInt32(stringBytes.Length);
            WriteBytes(stringBytes);
        }

        public void WriteInt64(long Item)
        {
            WriteToBufferPos(BitConverter.GetBytes(HostToNetworkOrder(Item)), 0, sizeof(long));
        }

        public void WriteUInt32(uint item)
        {
            WriteToBufferPos(BitConverter.GetBytes(HostToNetworkOrder(unchecked((int)item))), 0, sizeof(uint));
        }
        public void WriteInt32(int Item)
        {
            WriteToBufferPos(BitConverter.GetBytes(HostToNetworkOrder(Item)), 0, sizeof(int));
        }

        public void WriteInt16(short Item)
        {
            WriteToBufferPos(BitConverter.GetBytes(HostToNetworkOrder(Item)), 0, sizeof(short));
        }

        public void WriteBytes(byte[] Items)
        {
            WriteToBufferPos(Items, 0, Items.Length);
        }

        public void WriteBytes(byte[] Items, int Count)
        {
            WriteToBufferPos(Items, 0, Count);
        }

        public void WriteBytes(byte[] Items, int ItemOffset, int Count)
        {
            WriteToBufferPos(Items, ItemOffset, Count);
        }

        public void WriteInt64(long[] Items, int Offset, int Count)
        {
            if (Count > 0 && Items.Length >= Count)
            {
                int ByteOffset = Offset * sizeof(long);
                int ByteCount = Count * sizeof(long);

                for (int i = Offset; i < Count; i++)
                    Items[i] = HostToNetworkOrder(Items[i]);

                WriteToBufferPos(Items, ByteOffset, ByteCount);
            }
        }

        public void WriteInt32(int[] Items, int Offset, int Count)
        {
            if (Count > 0 && Items.Length >= Count)
            {
                int ByteOffset = Offset * sizeof(int);
                int ByteCount = Count * sizeof(int);

                for (int i = Offset; i < Count; i++)
                    Items[i] = HostToNetworkOrder(Items[i]);

                WriteToBufferPos(Items, ByteOffset, ByteCount);
            }
        }

        public void WriteInt16(short[] Items, int Offset, int Count)
        {
            if (Count > 0 && Items.Length >= Count)
            {
                int ByteOffset = Offset * sizeof(short);
                int ByteCount = Count * sizeof(short);

                for (int i = Offset; i < Count; i++)
                    Items[i] = HostToNetworkOrder(Items[i]);

                WriteToBufferPos(Items, ByteOffset, ByteCount);
            }
        }
        public void WriteInt16(ushort item)
        {
            WriteInt16((short)item);
        }

        #endregion Writer Methods

        #region Writer At Methods

        public void WriteStringAt(string Item, int Index)
        {
            WriteBytesAt(ASCIIEncoding.ASCII.GetBytes(Item), Index);
        }

        public void WriteInt64At(long Item, int Index)
        {
            WriteToBuffer(BitConverter.GetBytes(HostToNetworkOrder(Item)), 0, Index, sizeof(long));
        }

        public void WriteInt32At(int Item, int Index)
        {
            WriteToBuffer(BitConverter.GetBytes(HostToNetworkOrder(Item)), 0, Index, sizeof(int));
        }

        public void WriteInt16At(short Item, int Index)
        {
            WriteToBuffer(BitConverter.GetBytes(HostToNetworkOrder(Item)), 0, Index, sizeof(short));
        }

        public void WriteBytesAt(byte[] Items, int Index)
        {
            WriteToBuffer(Items, 0, Index, Items.Length);
        }

        public void WriteBytesAt(byte[] Items, int Index, int Count)
        {
            WriteToBuffer(Items, 0, Index, Count);
        }

        public void WriteBytesAt(byte[] Items, int ItemOffset, int Index, int Count)
        {
            WriteToBuffer(Items, ItemOffset, Index, Count);
        }

        public void WriteInt64At(long[] Items, int Offset, int Index, int Count)
        {
            if (Count > 0 && Items.Length >= Count)
            {
                int ByteOffset = Offset * sizeof(long);
                int ByteCount = Count * sizeof(long);

                for (int i = Offset; i < Count; i++)
                    Items[i] = HostToNetworkOrder(Items[i]);

                WriteToBuffer(Items, ByteOffset, Index, ByteCount);
            }
        }

        public void WriteInt32At(int[] Items, int Offset, int Index, int Count)
        {
            if (Count > 0 && Items.Length >= Count)
            {
                int ByteOffset = Offset * sizeof(int);
                int ByteCount = Count * sizeof(int);

                for (int i = Offset; i < Count; i++)
                    Items[i] = HostToNetworkOrder(Items[i]);

                WriteToBuffer(Items, ByteOffset, Index, ByteCount);
            }
        }

        public void WriteInt16At(short[] Items, int Offset, int Index, int Count)
        {
            if (Count > 0 && Items.Length >= Count)
            {
                int ByteOffset = Offset * sizeof(short);
                int ByteCount = Count * sizeof(short);

                for (int i = Offset; i < Count; i++)
                    Items[i] = HostToNetworkOrder(Items[i]);

                WriteToBuffer(Items, ByteOffset, Index, ByteCount);
            }
        }

        #endregion Writer At Methods

        #region WriteByte

        public void WriteByte(byte Item, int Index)
        {
            if ((m_ByteBuffer != null) && (m_ByteBuffer.Length > Index))
                m_ByteBuffer[Index] = Item;
        }

        #endregion WriteByte

        #region AllocateBufferSize

        public bool AllocateBufferSize(int Length)
        {
            if (Length < 0)
                return (false);

            if (m_ByteBuffer == null)
                m_ByteBuffer = new byte[Length];
            else if (m_ByteBuffer.Length < Length)
            {
                Array.Resize(ref m_ByteBuffer, Length);
            }

            return (m_ByteBuffer != null);
        }

        public bool AllocateBufferAdd(int Length)
        {
            return (AllocateBufferSize(m_CurrentPos + Length));
        }

        #endregion AllocateBuffer

        #region WriteToBuffer

        private void WriteToBuffer(Array Items, int ItemOffset, int Index, int Count)
        {
            if (AllocateBufferSize(Index + Count))
                Buffer.BlockCopy(Items, ItemOffset, m_ByteBuffer, Index, Count);
        }

        #endregion WriteToBuffer

        #region WriteToBufferPos

        private void WriteToBufferPos(Array Items, int ItemOffset, int Count)
        {
            WriteToBuffer(Items, ItemOffset, m_CurrentPos, Count);
            m_CurrentPos += Count;
        }

        #endregion WriteToBufferPos

        #region HostToNetworkOrder

        private short HostToNetworkOrder(short Item)
        {
            return (m_bUseNetworkByteOrder ? IPAddress.HostToNetworkOrder(Item) : Item);
        }

        private int HostToNetworkOrder(int Item)
        {
            return (m_bUseNetworkByteOrder ? IPAddress.HostToNetworkOrder(Item) : Item);
        }

        private long HostToNetworkOrder(long Item)
        {
            return (m_bUseNetworkByteOrder ? IPAddress.HostToNetworkOrder(Item) : Item);
        }

        #endregion HostToNetworkOrder
    }
}
