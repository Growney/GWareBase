using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public class clsHashProviderBase : IDisposable
    {
        #region constants

        public const HashAlgorithmType DefaultHashType = HashAlgorithmType.MD5;           //set default Algorithum type

        #endregion constants

        #region Static Methods

        public static HashAlgorithm GetHashAlgorithmFromType(HashAlgorithmType HashType)
        {
            if (HashType == HashAlgorithmType.Unknown)
                return (null);

            //set default hash algarithum here
            if (HashType == HashAlgorithmType.Default)
                HashType = clsHashProviderBase.DefaultHashType;

            if (HashType == HashAlgorithmType.SHA1)
                return (new SHA1Managed());
            else
                return (new MD5CryptoServiceProvider());
        }

        #endregion Static Methods

        #region Declarations

        protected HashAlgorithmType m_HashType = HashAlgorithmType.Unknown;
        protected HashAlgorithm m_HashAlg = null;

        #endregion Declarations

        #region construction

        public clsHashProviderBase(HashAlgorithmType HashType)
        {
            m_HashType = HashType;
            m_HashAlg = GetHashAlgorithmFromType(HashType);
        }

        #endregion Construction

        #region Properties

        public HashAlgorithmType HashType
        {
            get { return (m_HashType); }
        }

        public HashAlgorithm HashAlg
        {
            get { return (m_HashAlg); }
        }

        public int HashSize
        {
            get { return ((m_HashAlg != null) ? m_HashAlg.HashSize : 0); }
        }

        #endregion Properties

        #region Methods

        public byte[] ComputeHash(string TextString)
        {
            return (ComputeHash(EncryptDecryptBase.GetBytesFromString(TextString)));
        }

        public byte[] ComputeHash(byte[] hashbytes)
        {
            if (m_HashAlg != null)
                return (m_HashAlg.ComputeHash(hashbytes, 0, hashbytes.Length));

            return (null);
        }

        public byte[] ComputeHash(byte[] hashbytes, int Offset, int Count)
        {
            if (m_HashAlg != null)
                return (m_HashAlg.ComputeHash(hashbytes, Offset, Count));

            return (null);
        }

        public byte[] ComputeHash(Stream InputStream)
        {
            if (m_HashAlg != null)
                return (m_HashAlg.ComputeHash(InputStream));

            return (null);
        }

        #endregion Methods

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (m_HashAlg != null)
            {
                m_HashAlg.Clear();
                m_HashAlg = null;
            }
        }

        #endregion
    }
}
