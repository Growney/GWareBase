using Gware.Common.Application;
using Gware.Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public class UserAuthenticationKey
    {
        private readonly TimeSpan m_keyExpiry;
        private int m_userID;
        private string m_key;
        private DateTime m_creationDate;
        private DateTime m_expirationDate;
        private DateTime m_lastUpdate;

        public string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        public DateTime LastUpdate
        {
            get { return m_lastUpdate; }
            set { m_lastUpdate = value; }
        }

        public DateTime ExpirationDate
        {
            get { return m_expirationDate; }
            set { m_expirationDate = value; }
        }

        public DateTime CreationDate
        {
            get { return m_creationDate; }
            set { m_creationDate = value; }
        }

        public int UserID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }

        public bool IsAuthenticated
        {
            get
            {
                return (DateTime.UtcNow <= ExpirationDate.ToUniversalTime()) && (DateTime.UtcNow >= CreationDate.ToUniversalTime()) ;
            }
        }

        public bool IsTimedOut
        {
            get
            {
                return ExpirationDate.ToUniversalTime() > DateTime.UtcNow;
            }
        }

        public UserAuthenticationKey()
        {
            m_keyExpiry = TimeSpan.FromMinutes(1);
        }

        public static UserAuthenticationKey CreateKey(int userID,TimeSpan expiry,Encoding encoding,Random random, int size = 64)
        {
            DateTime creationDate = DateTime.UtcNow;
            DateTime expiryDate = creationDate + expiry;

            string key = GenerateAuthenticationToken(encoding,random,size);

            return new UserAuthenticationKey()
            {
                UserID = userID,
                Key = key,
                CreationDate = creationDate,
                ExpirationDate = expiryDate
            };
        }

        public static string GenerateAuthenticationToken(Encoding encoding, Random random,int size)
        {
            byte[] tokenBytes = new byte[size];
            random.NextBytes(tokenBytes);

            return encoding.GetString(tokenBytes);
        }
    }
}
