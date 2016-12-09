using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public class EncryptDecrypt : EncryptDecryptBase
    {
        #region construction

        public EncryptDecrypt(clsSystemKeyId KeyId)
        {
            if (KeyId == null)
                return;

            m_SystemKeyId = KeyId;
            InitPrivateKey(AutoEncryptDecrypt.GetSystemDerivedBytes(m_SystemKeyId));
        }

        public EncryptDecrypt(clsSystemKeyId KeyId, AutoEncryptDecrypt AED, CipherMode EncodingMode, int KeyLength)
        {
            if (KeyId == null)
                return;

            if (AED == null)
                AED = Application.ApplicationBase.EncryptDecrypt;

            m_SystemKeyId = KeyId;
            InitPrivateKey(AED.GetDeriveBytes(KeyId), EncodingMode, KeyLength);
        }

        public EncryptDecrypt(string Password, string Salt, DeriveKeyTypes DeriveUse)
        {
            InitCrypto(Password, Salt, 0, DeriveUse, CipherMode.CBC, c_KeySize);
        }

        public EncryptDecrypt(string Password, string Salt, int Iterations, DeriveKeyTypes DeriveUse, int KeyLength)
        {
            InitCrypto(Password, Salt, Iterations, DeriveUse, CipherMode.ECB, KeyLength);
        }

        public EncryptDecrypt(string Password, string Salt)
        {
            InitCrypto(Password, Salt, 0, DeriveKeyTypes.Rfc2898, CipherMode.CBC, c_KeySize);
        }

        public EncryptDecrypt(HashDeriveBytes hdb, CipherMode EncodingMode, int KeyLength)
        {
            try
            {
                m_SystemKeyId = null;

                InitPrivateKey(hdb, EncodingMode, KeyLength);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitCrypto(string Password, string Salt, int Iterations, DeriveKeyTypes DeriveUse, CipherMode EncodingMode, int KeyLength)
        {
            try
            {
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Salt))
                    return;

                if (Iterations < c_MinKeyIterations)
                    Iterations = c_DefaultKeyIterations;

                m_SystemKeyId = null;

                InitPrivateKey(new HashDeriveBytes(Password, Salt, Iterations, DeriveUse), EncodingMode, KeyLength);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion construction

        #region Static Methods

        public static EncryptDecrypt CreateEncryptDecrypt(byte[] DataKey)
        {
            EncryptDecrypt encdec = null;

            if (DataKey != null && DataKey.Length > 0)
            {
                byte[] Salt = (byte[])DataKey.Clone();
                Array.Reverse(Salt);
                HashDeriveBytes hdb = new HashDeriveBytes(DataKey, Salt, EncryptDecrypt.c_DefaultKeyIterations, DeriveKeyTypes.Rfc2898);
                encdec = new EncryptDecrypt(hdb, System.Security.Cryptography.CipherMode.CBC, EncryptDecrypt.c_KeySize);
            }

            return (encdec);
        }

        #endregion Static Methods

        #region Encryption Methods

        #region EncryptBlock

        public string EncryptBlock(string sDataToDecode, CipherMode modeType)
        {
            SetMode(modeType);
            return (EncryptBlock(sDataToDecode));
        }

        public string EncryptBlock(string sDataToDecode)
        {
            if (m_EncryptTransForm == null)
            {
                string ret = string.Empty;

                if (BeginEncrypt())
                {
                    ret = Encrypt(sDataToDecode);
                    EndEncrypt();
                }

                return (ret);
            }

            return (Encrypt(sDataToDecode));
        }

        public byte[] EncryptBlock(byte[] sDataToDecode)
        {
            if (m_EncryptTransForm == null)
            {
                byte[] ret = null;

                if (BeginEncrypt())
                {
                    ret = Encrypt(sDataToDecode);
                    EndEncrypt();
                }

                return (ret);
            }

            return (Encrypt(sDataToDecode));
        }

        #endregion EncryptBlock

        #region EncryptTo

        public string EncryptTo(byte[] sDataToEncode)
        {
            string sData = EncodeToString(Encrypt(sDataToEncode));
            if (m_SystemKeyId != null)
                return (clsSystemKeyId.EncodeSystemIndexs(m_SystemKeyId, sData));
            else
                return (sData);
        }

        public byte[] EncryptTo(string sDataToEncode)
        {
            return (Encrypt(GetBytesFromString(sDataToEncode)));
        }

        #endregion EncryptTo

        #region Encrypt

        public string Encrypt(string sDataToEncode)
        {
            sDataToEncode = EncodeToString(Encrypt(GetBytesFromString(sDataToEncode)));
            if (m_SystemKeyId != null)
                return (clsSystemKeyId.EncodeSystemIndexs(m_SystemKeyId, sDataToEncode));
            else
                return (sDataToEncode);
        }

        public byte[] Encrypt(byte[] sDataToEncode)
        {
            byte[] retData = null;

            try
            {
                if (m_EncryptTransForm != null)
                    retData = m_EncryptTransForm.TransformFinalBlock(sDataToEncode, 0, sDataToEncode.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (retData);
        }

        #endregion Encrypt

        #region EncryptFile

        public bool EncryptFile(string InFileName, string OutFileName)
        {
            bool res = false;
            using (FileStream fsIn = File.Open(InFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fsOut = File.Open(OutFileName, FileMode.Create, FileAccess.Write))
                {
                    res = EncryptStream(fsIn, fsOut);
                    fsOut.Flush();
                    fsOut.Close();
                }

                fsIn.Close();
            }

            return (res);
        }

        #endregion EncryptFile

        #region EncryptToFile

        public bool EncryptToFile(Stream Data, string FileName)
        {
            bool res = false;

            using (FileStream fs = File.Open(FileName, FileMode.Create, FileAccess.Write))
            {
                res = EncryptStream(Data, fs);
            }

            return (res);
        }

        public bool EncryptToFile(byte[] Data, string FileName)
        {
            bool ret = false;

            using (MemoryStream DataStream = new MemoryStream(Data, false))
            {
                ret = EncryptToFile(DataStream, FileName);
            }

            return (ret);
        }

        #endregion EncryptToFile

        #region EncryptStream

        public bool EncryptStream(Stream DataStream, Stream OutputStream)
        {
            bool ret = false;
            if (BeginEncrypt())
            {
                if (m_SystemKeyId != null)
                {
                    byte[] KeyBytes = m_SystemKeyId.EncodedKeyBytes;
                    DataStream.Write(KeyBytes, 0, KeyBytes.Length);
                }
                using (CryptoStream cStream = new CryptoStream(OutputStream, m_EncryptTransForm, CryptoStreamMode.Write))
                {
                    byte[] DataBuff = new byte[m_FileBufferLen];
                    int DataLen = 0;

                    do
                    {
                        DataLen = DataStream.Read(DataBuff, 0, DataBuff.Length);
                        if (DataLen > 0)
                            cStream.Write(DataBuff, 0, DataLen);
                    }
                    while (DataLen > 0);

                    cStream.Flush();
                    OutputStream.Flush();
                    cStream.Close();
                    ret = true;
                }

                EndEncrypt();
            }

            return (ret);
        }

        #endregion EncryptStream

        #endregion Encryption Methods

        #region Decryption Methods

        #region DecryptBlock

        public string DecryptBlock(string sDataToDecode, CipherMode modeType)
        {
            SetMode(modeType);
            return (DecryptBlock(sDataToDecode));
        }

        public string DecryptBlock(string sDataToDecode)
        {
            if (m_DecryptTransForm == null)
            {
                string ret = string.Empty;

                if (BeginDecrypt())
                {
                    ret = Decrypt(sDataToDecode);
                    EndDecrypt();
                }

                return (ret);
            }

            return (Decrypt(sDataToDecode));
        }

        public byte[] DecryptBlock(byte[] sDataToDecode)
        {
            if (m_DecryptTransForm == null)
            {
                byte[] ret = null;

                if (BeginDecrypt())
                {
                    ret = Decrypt(sDataToDecode);
                    EndDecrypt();
                }

                return (ret);
            }

            return (Decrypt(sDataToDecode));
        }

        #endregion DecryptBlock

        #region DecryptTo

        public byte[] DecryptTo(string sDataToDecode)
        {
            if (m_SystemKeyId != null)
                sDataToDecode = clsSystemKeyId.StripSystemIndexs(sDataToDecode);

            return (Decrypt(DecodeFromString(sDataToDecode)));
        }

        public string DecryptTo(byte[] sDataToDecode)
        {
            return (ConvertToASCIIString(Decrypt(sDataToDecode)));
        }

        #endregion DecryptTo

        #region Decrypt

        public string Decrypt(string sDataToDecode)
        {
            if (m_SystemKeyId != null)
                sDataToDecode = clsSystemKeyId.StripSystemIndexs(sDataToDecode);

            return (ConvertToASCIIString(Decrypt(DecodeFromString(sDataToDecode))));
        }

        public byte[] Decrypt(byte[] sDataToDecode)
        {
            byte[] retData = null;
            try
            {
                if (m_DecryptTransForm != null && sDataToDecode != null && sDataToDecode.Length > 0)
                    retData = m_DecryptTransForm.TransformFinalBlock(sDataToDecode, 0, sDataToDecode.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (retData);
        }

        #endregion Decrypt

        #region DecryptFile

        public bool DecryptFile(string InFileName, string OutFileName)
        {
            bool res = false;
            using (FileStream fsIn = File.Open(InFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fsOut = File.Open(OutFileName, FileMode.Create, FileAccess.Write))
                {
                    res = DecryptStream(fsIn, fsOut);
                    fsOut.Flush();
                    fsOut.Close();
                }

                fsIn.Close();
            }

            return (res);
        }

        #endregion DecryptFile

        #region DecryptFromFile

        public bool DecryptFromFile(string FileName, byte[] Data)
        {
            bool ret = false;

            using (MemoryStream DataStream = new MemoryStream(Data, true))
            {
                ret = DecryptFromFile(FileName, DataStream);
            }

            return (ret);
        }

        public bool DecryptFromFile(string FileName, Stream Data)
        {
            bool res = false;

            using (FileStream fs = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.Read))
            {
                res = DecryptStream(fs, Data);
                fs.Close();
            }

            return (res);
        }

        #endregion DecryptFromFile

        #region DecryptStream

        public bool DecryptStream(Stream DataStream, Stream OutputStream)
        {
            bool ret = false;

            if (m_PrivateKey == null)
            {
                byte[] Indexs = new byte[clsSystemKeyId.c_IndexLen];

                DataStream.Read(Indexs, 0, Indexs.Length);   //base64 encoded string
                m_SystemKeyId = new clsSystemKeyId(Indexs.ToString());

                InitPrivateKey(AutoEncryptDecrypt.GetSystemDerivedBytes(m_SystemKeyId));
            }

            if (BeginDecrypt())
            {
                using (CryptoStream cStream = new CryptoStream(DataStream, m_DecryptTransForm, CryptoStreamMode.Read))
                {
                    byte[] DataBuff = new byte[m_FileBufferLen];
                    int DataLen = 0;

                    do
                    {
                        DataLen = cStream.Read(DataBuff, 0, DataBuff.Length);
                        if (DataLen > 0)
                            OutputStream.Write(DataBuff, 0, DataLen);
                    }
                    while (DataLen > 0);

                    OutputStream.Flush();
                    cStream.Close();

                    ret = true;
                }

                EndDecrypt();
            }

            return (ret);
        }

        #endregion DecryptStream

        #endregion Decryption Methods
    }
}
