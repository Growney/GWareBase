using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public class clsSystemKeyData
    {
        public const int c_MaxDataLen = 32;
        private static readonly string c_AppLockString = "_TAB%349";

        private DeriveKeyTypes m_Keytype;
        private int m_Iterations;

        private byte[][] c_KeyData = null;

        public clsSystemKeyData()
        {
            GenerateKeyMaterial(null, HashDeriveBytes.c_DefaultDeriveKeyType, HashDeriveBytes.c_DefaultIterations);
        }

        public clsSystemKeyData(HashDeriveBytes hd)
        {
            GenerateKeyMaterial(hd, HashDeriveBytes.c_DefaultDeriveKeyType, HashDeriveBytes.c_DefaultIterations);
        }

        public clsSystemKeyData(HashDeriveBytes hd, DeriveKeyTypes type, int Iterations)
        {
            GenerateKeyMaterial(hd, type, Iterations);
        }

        private void GenerateKeyMaterial(HashDeriveBytes hd, DeriveKeyTypes type, int Iterations)
        {
            m_Keytype = type;
            m_Iterations = Iterations;

            if (hd == null)
                hd = clsSystemKeyData.GenerateSystemKeyMaterial(c_AppLockString);

            hd.Reset();

            c_KeyData = new byte[4][];
            for (int i = 0; i < c_KeyData.Length; i++)
            {
                c_KeyData[i] = hd.GetBytes(c_MaxDataLen);
            }

            hd.Clear();
        }

        public byte[] GenerateSystemKey(clsSystemKeyId keyId)
        {
            return (GenerateSystemKeys(keyId, false));
        }

        public byte[] GenerateSystemSalt(clsSystemKeyId keyId)
        {
            return (GenerateSystemKeys(keyId, true));
        }

        public string GenerateSystemKeyString(clsSystemKeyId keyId)
        {
            return (Convert.ToBase64String(GenerateSystemKey(keyId)));
        }

        public string GenerateSystemSaltString(clsSystemKeyId keyId)
        {
            return (Convert.ToBase64String(GenerateSystemSalt(keyId)));
        }

        public HashDeriveBytes GetDeriveBytes(clsSystemKeyId keyId)
        {
            return (new HashDeriveBytes(GenerateSystemKey(keyId), GenerateSystemSalt(keyId), m_Iterations, m_Keytype));
        }

        private byte[] GenerateSystemKeys(clsSystemKeyId keyId, bool Reverse)
        {
            if (keyId == null)
                return (null);

            if (c_KeyData == null)
                GenerateKeyMaterial(null, HashDeriveBytes.c_DefaultDeriveKeyType, HashDeriveBytes.c_DefaultIterations);

            byte[] Data = new byte[c_KeyData.Length * 8];

            int length = c_KeyData.Length - 1;
            int blocklen = c_KeyData[0].Length - 2;

            int Index1 = (Reverse ? keyId.Index2 : keyId.Index1);
            int Index2 = (Reverse ? keyId.Index1 : keyId.Index2);

            if ((Index1 < blocklen) && (Index2 < blocklen))
            {
                for (int i = 0; i < c_KeyData.Length; i++)
                {
                    //base
                    int x = (8 * i);

                    Array.Copy(c_KeyData[i], blocklen - Index1, Data, x, 2);
                    Array.Copy(c_KeyData[i], Index2, Data, (x + 2), 2);
                    Array.Copy(c_KeyData[i], blocklen - Index2, Data, (x + 4), 2);
                    Array.Copy(c_KeyData[i], Index1, Data, (x + 6), 2);
                }
            }

            return (Data);
        }

        private static HashDeriveBytes GenerateSystemKeyMaterial(string sAppLockString)
        {
            string sPKey = Gware.Common.Application.ApplicationBase.ApplicationCompanyName;
            sPKey += sAppLockString;
            string sSKey = Gware.Common.Application.ApplicationBase.ApplicationTitle;
            sPKey += sAppLockString;

            return (new HashDeriveBytes(sPKey, sSKey, HashDeriveBytes.c_DefaultIterations, DeriveKeyTypes.Rfc2898));
        }

        public void Clear()
        {
            for (int i = 0; i < c_KeyData.Length; i++)
            {
                Array.Clear(c_KeyData[i], 0, c_KeyData[i].Length);
            }
        }
    }
}
