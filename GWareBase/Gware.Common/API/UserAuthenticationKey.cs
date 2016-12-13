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
        private static readonly TimeSpan c_keyExpiry = TimeSpan.FromHours(1);

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

        }

        public static UserAuthenticationKey CreateKey(int userID)
        {
            DateTime creationDate = DateTime.UtcNow;
            DateTime expiryDate = creationDate + c_keyExpiry;

            string key = ApplicationBase.GenerateAuthenticationToken();

            return new UserAuthenticationKey()
            {
                UserID = userID,
                Key = key,
                CreationDate = creationDate,
                ExpirationDate = expiryDate
            };
        }
    }
}
