using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Encryption
{
    public static class MD5Hash
    {
        private static readonly Encoding c_applicationEncoding = Encoding.Unicode;

        public static string GetMD5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hasher.ComputeHash(c_applicationEncoding.GetBytes(input));

            string hashedString = c_applicationEncoding.GetString(data);

            return hashedString;
        }
        public static bool VerifyMD5Hash(string input, string hash)
        {
            string inputMD5 = GetMD5Hash(input);

            return inputMD5.Equals(inputMD5);
        }
    }
}
