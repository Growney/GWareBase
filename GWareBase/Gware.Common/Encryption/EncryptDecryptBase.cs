using Gware.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public enum HashAlgorithmType
    {
        Unknown = -1,
        Default = 0,
        MD5 = 1,
        SHA1 = 2
    }
    public class EncryptDecryptBase
    {
        #region Constants

        //----------------------------------------------------------------------------------------------------
        public const int c_MinKeyIterations = 1000;
        public const int c_DefaultKeyIterations = 1024;
        public const int c_IVSize = 16;
        public const int c_KeySize = 32;
        public const int c_MinFileBufferSize = 1024;
        public const int c_MaxFileBufferSize = (c_MinFileBufferSize * 32);

        public const string c_PasswordHashFormat = "{0}-OX{1}XO-{2}";

        //----------------------------------------------------------------------------------------------------

        #endregion Constants

        #region Declarations

        protected SymmetricAlgorithm m_Crypto = null;
        protected HashDeriveBytes m_PrivateKey = null;
        protected ICryptoTransform m_EncryptTransForm = null;
        protected ICryptoTransform m_DecryptTransForm = null;

        protected clsSystemKeyId m_SystemKeyId = null;

        protected int m_FileBufferLen = c_MaxFileBufferSize;
        protected int m_KeyLength = c_KeySize;

        #endregion Declarations

        #region Construction

        public EncryptDecryptBase()
        {
            m_Crypto = (SymmetricAlgorithm)new RijndaelManaged();
        }

        public EncryptDecryptBase(SymmetricAlgorithm Crypto)
        {
            m_Crypto = Crypto;
        }

        protected void InitPrivateKey(HashDeriveBytes PrivateKey)
        {
            InitPrivateKey(PrivateKey, CipherMode.CBC, c_KeySize);
        }

        protected void InitPrivateKey(HashDeriveBytes PrivateKey, CipherMode EncodingMode, int KeyLength)
        {
            m_PrivateKey = PrivateKey;
            m_Crypto.Mode = EncodingMode;
            m_KeyLength = KeyLength;
        }

        #endregion Construction

        #region Methods

        public int FileBufferLen
        {
            get { return (m_FileBufferLen); }
            set
            {
                if (value > (c_MinFileBufferSize - 1))
                    m_FileBufferLen = value;
                else
                    m_FileBufferLen = c_MinFileBufferSize;
            }
        }

        public void SetMode(CipherMode modeType)
        {
            if (m_Crypto != null)
                m_Crypto.Mode = modeType;
        }

        #endregion Methods

        #region BeginEncrypt

        public bool BeginEncrypt()
        {
            return ((m_Crypto != null) && BeginEncrypt(m_Crypto.Mode));
        }

        public bool BeginEncrypt(CipherMode modeType)
        {
            try
            {
                EndEncrypt();

                if (m_Crypto != null)
                {
                    m_Crypto.Mode = modeType;

                    if (m_PrivateKey == null)
                    {
                        m_SystemKeyId = new clsSystemKeyId();
                        m_PrivateKey = AutoEncryptDecrypt.GetSystemDerivedBytes(m_SystemKeyId);
                    }

                    m_PrivateKey.Reset();

                    byte[] keyBytes = m_PrivateKey.GetBytes(m_KeyLength);
                    byte[] bVector = m_PrivateKey.GetVectorBytes(c_IVSize);

                    //m_Crypto.KeySize = (m_KeyLength * 8); //BB_TODO!: set len here for CCCryto support?

                    m_EncryptTransForm = m_Crypto.CreateEncryptor(keyBytes, bVector);

                    return (m_EncryptTransForm != null);
                }
            }
            catch (Exception /*ex*/)
            {
                //throw ex;
            }

            return (false);
        }

        #endregion BeginEncrypt

        #region BeginDecrypt

        public bool BeginDecrypt()
        {
            return ((m_Crypto != null) && BeginDecrypt(m_Crypto.Mode));
        }

        public bool BeginDecrypt(CipherMode modeType)
        {
            try
            {
                EndDecrypt();

                if (m_Crypto != null && m_PrivateKey != null)
                {
                    m_Crypto.Mode = modeType;

                    m_PrivateKey.Reset();

                    byte[] keyBytes = m_PrivateKey.GetBytes(m_KeyLength);
                    byte[] bVector = m_PrivateKey.GetVectorBytes(c_IVSize);

                    m_DecryptTransForm = m_Crypto.CreateDecryptor(keyBytes, bVector);

                    return (m_DecryptTransForm != null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (false);
        }

        #endregion BeginDecrypt

        #region Static Methods

        #region Encode / Decode Strings

        public static string ConvertToASCIIString(byte[] ByteData)
        {
            if (ByteData != null)
                return (Encoding.ASCII.GetString(ByteData, 0, ByteData.Length));
            else
                return (string.Empty);
        }

        public static string ConvertToUnicodeString(byte[] ByteData)
        {
            if (ByteData != null)
                return (Encoding.Unicode.GetString(ByteData, 0, ByteData.Length));
            else
                return (string.Empty);
        }

        public static byte[] GetBytesFromString(string Data)
        {
            if (Data != null)
                return (Encoding.ASCII.GetBytes(Data));
            else
                return (null);
        }

        public static string EncodeToString(byte[] ByteDataToEncode)
        {
            if (ByteDataToEncode != null)
                return (Convert.ToBase64String(ByteDataToEncode));
            else
                return (string.Empty);
        }

        public static byte[] DecodeFromString(string StringDataToDecode)
        {
            try
            {
                if (StringDataToDecode != null)
                    return (Convert.FromBase64String(StringDataToDecode));

            }
            catch (Exception)
            {
            }
            return (null);
        }

        #endregion Encode / Decode Strings

        #region MakePasswordHash

        public static byte[] MakePasswordHash(string userName, string Pwd)
        {
            return (MakePasswordHash(userName, Pwd, HashAlgorithmType.Default));
        }

        public static byte[] MakePasswordHash(string userName, string Pwd, HashAlgorithmType HashType)
        {
            string sProductName = ApplicationBase.ApplicationTitle;

            return (MakePasswordHash(userName, Pwd, sProductName, HashType));
        }

        public static byte[] MakePasswordHash(string userName, string Pwd, string sProductName, HashAlgorithmType HashType)
        {
            clsHashProviderBase HashAlg = new clsHashProviderBase(HashType);
            if (HashAlg != null)
                using (HashAlg)
                {
                    return (MakePasswordHash(userName, Pwd, sProductName, HashAlg));
                }

            return (null);
        }

        public static byte[] MakePasswordHash(string userName, string Pwd, string sProductName, clsHashProviderBase HashAlg)
        {
            byte[] pwdbytes = ASCIIEncoding.ASCII.GetBytes(string.Format(c_PasswordHashFormat, userName.ToLower(), Pwd, sProductName.ToLower()));

            if (HashAlg != null)
                return (HashAlg.ComputeHash(pwdbytes));

            return (null);
        }

        #endregion MakePasswordHash

        #endregion Static Methods

        #region IDisposable Members

        public void EndEncrypt()
        {
            if (m_EncryptTransForm != null)
            {
                m_EncryptTransForm.Dispose();
                m_EncryptTransForm = null;
            }
        }

        public void EndDecrypt()
        {
            if (m_DecryptTransForm != null)
            {
                m_DecryptTransForm.Dispose();
                m_DecryptTransForm = null;
            }
        }

        public void Dispose()
        {
            EndEncrypt();
            EndDecrypt();

            if (m_PrivateKey != null)
                m_PrivateKey = null;

            if (m_Crypto != null)
            {
                m_Crypto.Clear();
                m_Crypto = null;
            }
        }

        #endregion IDisposable Members
    }
}
