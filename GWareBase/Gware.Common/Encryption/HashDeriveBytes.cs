using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public enum DeriveKeyTypes
    {
        Rfc2898 = 0,
        PasswordBytesSHA1 = 1,
        PasswordBytesMD5 = 2,
        MD5 = 3,
        SHA1 = 4
    }

    public class HashDeriveBytes : DeriveBytes
    {
        public const int c_DefaultIterations = 1024;
        public const DeriveKeyTypes c_DefaultDeriveKeyType = DeriveKeyTypes.Rfc2898;

        private int m_Pos = 0;

        private DeriveBytes m_Internal = null;
        private DeriveKeyTypes m_HashType = DeriveKeyTypes.Rfc2898;

        private byte[] m_ByteMaterial = null;

        public HashDeriveBytes(byte[] Password, byte[] Salt, int Iterations)
            : base()
        {
            InitGenerateDeriveBytes(Password, Salt, Iterations, DeriveKeyTypes.SHA1);
        }

        public HashDeriveBytes(byte[] Password, byte[] Salt, int Iterations, DeriveKeyTypes type)
            : base()
        {
            InitGenerateDeriveBytes(Password, Salt, Iterations, type);
        }

        public HashDeriveBytes(string Password, string Salt, int Iterations, DeriveKeyTypes type)
            : base()
        {
            InitGenerateDeriveBytes(Encoding.ASCII.GetBytes(Password), Encoding.ASCII.GetBytes(Salt), Iterations, type);
        }

        private void InitGenerateDeriveBytes(byte[] Password, byte[] Salt, int Iterations, DeriveKeyTypes type)
        {
            clsHashProviderBase hashAlg = null;

            if (type == DeriveKeyTypes.Rfc2898)
            {
                m_Internal = new Rfc2898DeriveBytes(Password, Salt, Iterations);
                return;
            }
            else if (type == DeriveKeyTypes.PasswordBytesSHA1)
            {
                m_Internal = new PasswordDeriveBytes(Password, Salt, "SHA1", Iterations);
                return;
            }
            else if (type == DeriveKeyTypes.PasswordBytesMD5)
            {
                m_Internal = new PasswordDeriveBytes(Password, Salt, "MD5", Iterations);
                return;
            }
            else if (type == DeriveKeyTypes.SHA1)
            {
                hashAlg = new clsHashProviderBase(HashAlgorithmType.SHA1);
            }
            else
            {
                hashAlg = new clsHashProviderBase(HashAlgorithmType.MD5);
            }

            if (hashAlg != null)
            {
                using (hashAlg)
                {
                    int HashSize = hashAlg.HashSize / 8;
                    int HalfHashSize = HashSize / 2;

                    m_ByteMaterial = new byte[HashSize * 4];

                    byte[][] dataa = new byte[4][];

                    dataa[0] = hashAlg.ComputeHash(Password, 0, Password.Length);
                    dataa[1] = hashAlg.ComputeHash(Salt, 0, Salt.Length);
                    dataa[2] = (byte[])dataa[0].Clone();
                    dataa[3] = (byte[])dataa[1].Clone();

                    Array.Reverse(dataa[2], 0, dataa[2].Length);
                    Array.Reverse(dataa[3], 0, dataa[3].Length);

                    for (int i = 0; i < Iterations; i++)
                    {
                        for (int j = 0; j < dataa.Length; j++)
                            dataa[j] = hashAlg.ComputeHash(dataa[j], 0, dataa[j].Length);
                    }

                    for (int i = 0; i < (dataa.Length * 2); i++)
                        Buffer.BlockCopy(dataa[i % 4], ((i / 4) * HalfHashSize), m_ByteMaterial, (HalfHashSize * i), HalfHashSize);

                    for (int i = 2; i < m_ByteMaterial.Length - 8; i += 8)
                        Array.Reverse(m_ByteMaterial, i, 4);

                    for (int i = 0; i < dataa.Length; i++)
                        Array.Clear(dataa[i], 0, dataa[i].Length);
                }
            }
            m_Pos = 0;
        }

        public DeriveKeyTypes HashType
        {
            get { return (m_HashType); }
        }

        public virtual byte[] GetVectorBytes(int cb)
        {
            return (GetBytes(cb));
        }

        public override byte[] GetBytes(int cb)
        {
            if (m_Internal != null)
                return (m_Internal.GetBytes(cb));

            byte[] data = new byte[cb];
            int iCountLeft = cb;

            while (iCountLeft > 0)
            {
                int len = (m_ByteMaterial.Length - m_Pos);

                if (len > iCountLeft)
                    len = iCountLeft;

                Array.Copy(m_ByteMaterial, m_Pos, data, (cb - iCountLeft), len);

                m_Pos += len;
                iCountLeft -= len;

                if (m_Pos > m_ByteMaterial.Length - 1)
                    m_Pos = 0;
            }

            return (data);
        }

        public override void Reset()
        {
            m_Pos = 0;
            if (m_Internal != null)
                m_Internal.Reset();
        }

        public void Clear()
        {
            Reset();

            m_Internal = null;

            if (m_ByteMaterial != null)
                Array.Clear(m_ByteMaterial, 0, m_ByteMaterial.Length);
        }
    }
}
