using Gware.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Security
{
    [Flags]
    public enum PasswordViolation
    {
        None = 0x00,
        Digits = 0x01,
        Letters = 0x02,
        Special = 0x04,
        MinumumLenght = 0x08,
        MaximumLength = 0x10
    }
    public static class SecurityHelper
    {
        private static Random m_random;

        static SecurityHelper()
        {
            m_random = new Random();
        }
        public static PasswordViolation CheckPasswordStatus(string password, int minimumDigits, int minimumLetters, int minimumSpecialCharacters, int minimumLength, int maximumLength)
        {
            PasswordViolation retVal = PasswordViolation.None;

            int[] passwordCounts = GetPasswordCounts(password);

            if (passwordCounts[0] < minimumDigits || minimumDigits == 0)
            {
                retVal |= PasswordViolation.Digits;
            }
            if (passwordCounts[1] < minimumLetters || minimumLetters == 0)
            {
                retVal |= PasswordViolation.Digits;
            }
            if (passwordCounts[2] < minimumSpecialCharacters || minimumSpecialCharacters == 0)
            {
                retVal |= PasswordViolation.Special;
            }
            if (passwordCounts[3] < minimumLength || minimumLength == 0)
            {
                retVal |= PasswordViolation.MinumumLenght;
            }
            if (passwordCounts[3] > minimumLength || minimumLength == 0)
            {
                retVal |= PasswordViolation.MaximumLength;
            }

            return retVal;
        }
        public static bool CheckPassword(string password, int minimumDigits, int minimumLetters, int minimumSpecialCharacters, int minimumLength, int maximumLength)
        {
            return CheckPasswordStatus(password, minimumDigits, minimumLetters, minimumSpecialCharacters, minimumLength, maximumLength) == PasswordViolation.None;
        }
        public static int[] GetPasswordCounts(string password)
        {
            int digitCount = 0;
            int letterCount = 0;
            int specialDigitCount = 0;
            int lenght = password.Length;

            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsDigit(password[i]))
                {
                    digitCount++;
                }

                if (char.IsLetter(password[i]))
                {
                    letterCount++;
                }

                if (char.IsLetterOrDigit(password[i]))
                {
                    specialDigitCount++;
                }
            }

            return new int[] { digitCount, letterCount, specialDigitCount, lenght };
        }

        public static string CreateKey(int length)
        {
            byte[] data = new byte[length];
            m_random.NextBytes(data);

            return Encoding.ASCII.GetString(data);
        }
    }
}
