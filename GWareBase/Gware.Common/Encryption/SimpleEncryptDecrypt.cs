using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace Gware.Common.Encryption
{
    public static class SimpleEncryptDecrypt
    {
        private static readonly Encoding c_encoding = Encoding.UTF32;

        public static string Encrypt(string value)
        {
            byte[] stringBytes = c_encoding.GetBytes(value);
            return Encrypt(stringBytes);
        }
        public static string Encrypt(byte[] value)
        {
            using(DESCryptoServiceProvider service = new DESCryptoServiceProvider())
            {
                service.GenerateKey();
                byte[] key = service.Key;

                ICryptoTransform encryptor = service.CreateEncryptor();

                byte[] enc = encryptor.TransformFinalBlock(value, 0, value.Length);

                return c_encoding.GetString(enc);
            }
        }

        public static string Decrypt(string value)
        {
            byte[] stringBytes = c_encoding.GetBytes(value);
            return Decrypt(stringBytes);
        }
        public static string Decrypt(byte[] value)
        {
            using (DESCryptoServiceProvider service = new DESCryptoServiceProvider())
            {
                service.GenerateKey();
                byte[] key = service.Key;

                ICryptoTransform decryptor = service.CreateDecryptor();

                byte[] enc = decryptor.TransformFinalBlock(value, 0, value.Length);

                return c_encoding.GetString(enc);
            }
        }
    }
}
