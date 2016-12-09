using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public class AutoEncryptDecrypt
    {
        private clsSystemKeyData m_SystemKeyData = null;

        public AutoEncryptDecrypt()
        {
            InitCrypto(null);
        }

        public AutoEncryptDecrypt(DeriveKeyTypes dkt, int Iterations)
        {
            InitCrypto(new clsSystemKeyData(null, dkt, Iterations));
        }

        public AutoEncryptDecrypt(clsSystemKeyData skd)
        {
            InitCrypto(skd);
        }

        private void InitCrypto(clsSystemKeyData skd)
        {
            if (skd == null)
                m_SystemKeyData = new clsSystemKeyData();
        }

        public HashDeriveBytes GetDeriveBytes(clsSystemKeyId keyId)
        {
            if (m_SystemKeyData != null)
                return (m_SystemKeyData.GetDeriveBytes(keyId));

            return (GetSystemDerivedBytes(keyId));
        }

        public static HashDeriveBytes GetSystemDerivedBytes(clsSystemKeyId keyId)
        {
            return (Application.ApplicationBase.EncryptDecrypt.GetDeriveBytes(keyId));
        }

        public void Clear()
        {
            if (m_SystemKeyData != null)
                m_SystemKeyData.Clear();
        }
    }
}
