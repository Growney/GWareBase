using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public class clsSystemKeyId
    {
        #region Constants

        public const int c_IndexLen = 4;

        #endregion Constants

        #region Declaration

        private byte[] m_Indexs = new byte[2];

        #endregion Declaration

        #region Construction

        public clsSystemKeyId()
        {
            GenerateSystemIndexs();
        }

        public clsSystemKeyId(string Data)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                try
                {
                    byte[] Indexs = Convert.FromBase64String(Data.Substring(0, c_IndexLen));
                    SetIndexs(Indexs);
                }
                catch (Exception)
                { }
            }
        }

        public clsSystemKeyId(byte Index1, byte Index2)
        {
            m_Indexs[0] = Index1;
            m_Indexs[1] = Index2;
        }

        public clsSystemKeyId(byte[] Indexs)
        {
            SetIndexs(Indexs);
        }

        #endregion Construction

        #region Private Helpers

        private void SetIndexs(byte[] Indexs)
        {
            Buffer.BlockCopy(Indexs, 0, m_Indexs, 0, m_Indexs.Length);
        }

        private void GenerateSystemIndexs()
        {
            int MaxLen = clsSystemKeyData.c_MaxDataLen - 2;

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < m_Indexs.Length; i++)
                m_Indexs[i] = (byte)rnd.Next(MaxLen);
        }

        #endregion Private Helpers

        #region Properties

        public byte Index1
        {
            get { return (m_Indexs[0]); }
        }

        public byte Index2
        {
            get { return (m_Indexs[1]); }
        }

        public string EncodedKey
        {
            get { return (Convert.ToBase64String(m_Indexs)); }
        }

        public byte[] EncodedKeyBytes
        {
            get { return (ASCIIEncoding.ASCII.GetBytes(this.EncodedKey)); }
        }

        public byte[] RawKeys
        {
            get { return (m_Indexs); }
        }

        #endregion Properties

        #region Static Helpers

        public static string EncodeSystemIndexs(clsSystemKeyId keyId, string Data)
        {
            return (string.Format("{0}{1}", keyId.EncodedKey, Data));
        }

        public static string StripSystemIndexs(string Data)
        {
            return (Data.Substring(c_IndexLen));
        }

        #endregion Static Helpers
    }
}
