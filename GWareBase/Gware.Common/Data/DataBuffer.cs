using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Data
{
    public class DataBuffer
    {
        #region declarations

        protected byte[] m_DataBuff = null;
        protected int m_Datalength = 0;

        #endregion declarations

        #region construction

        public DataBuffer(int BufferSize)
        {
            if (BufferSize < 1)
                BufferSize = 20;

            m_DataBuff = new byte[BufferSize];
        }

        #endregion construction

        #region properties

        public int Length
        {
            get { return (m_Datalength); }
        }

        public byte[] ByteBuffer
        {
            get { return (m_DataBuff); }
        }

        public byte this[int i]
        {
            get
            {
                if (i < 0 || i > m_Datalength)
                    return (0);

                return (m_DataBuff[i]);
            }
            set
            {
                if (i < 0 || i > m_Datalength)
                    m_DataBuff[i] = value;
            }
        }

        #endregion properties

        #region methods

        public bool ExtendBuffer(int Offset)
        {
            return (AlocateBuffer(m_Datalength + Offset));
        }

        public bool AlocateBuffer(int Length)
        {
            try
            {
                if (m_DataBuff.Length < Length)
                    Array.Resize<byte>(ref m_DataBuff, Length);

                return (true);
            }
            catch (Exception)
            {
                return (false);
            }
        }

        public void IncLength(int Offset)
        {
            SetLength(m_Datalength + Offset);
        }

        public void SetLength(int Length)
        {
            if (Length < 1)
                m_Datalength = 0;
            else if (m_DataBuff.Length > Length)
                m_Datalength = Length;
            else
                m_Datalength = m_DataBuff.Length;
        }

        public bool Append(byte[] buff)
        {
            if (buff != null)
            {
                int len = buff.Length;
                if (len > 0)
                {
                    if (AlocateBuffer(m_Datalength + len))
                    {
                        Buffer.BlockCopy(buff, 0, m_DataBuff, m_Datalength, len);

                        m_Datalength += len;

                        return (true);
                    }
                }
            }

            return (false);
        }

        public byte[] GetBytes(int Start, int Length)
        {
            if (Start < 0 || Length < 1 || (m_Datalength < (Start + Length)))
                return (null);

            byte[] buff = new byte[Length];
            Buffer.BlockCopy(m_DataBuff, Start, buff, 0, Length);

            return (buff);
        }

        public byte[] GetBytes()
        {
            return (GetBytes(0, m_Datalength));
        }

        public void Clear()
        {
            ClearTo(-1);
        }

        public void ClearTo(int Pos)
        {
            int len;

            if (Pos == 0)
                return;

            if (Pos > 0)
            {
                len = (m_Datalength - Pos);
                if (len > 0)
                {
                    byte[] newbuff = new byte[len];
                    Buffer.BlockCopy(m_DataBuff, Pos, newbuff, 0, len);
                    Buffer.BlockCopy(newbuff, 0, m_DataBuff, 0, len);
                }
                else
                    len = 0;
            }
            else
                len = 0;

            if (len < m_Datalength)
                Array.Clear(m_DataBuff, len, (m_Datalength - len));

            m_Datalength = len;
        }

        #endregion methods
    }
}
