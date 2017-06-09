using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;

namespace Gware.Common.Security.Authentication
{
    public enum eTokenError
    {
        None = -1,
        Error = 0,
        UsernameDoesNotExist = 1,
        IncorrectPassword = 2,
        AccountLocked = 3,
    }
    public class AuthenticationToken : Storage.LoadedFromAdapterBase
    {
        private DateTime m_expiry;
        private string m_key;
        private DateTime m_created;
        private int m_userID;
        private int m_errorCode;

        public DateTime Expiry
        {
            get
            {
                return m_expiry;
            }

            set
            {
                m_expiry = value;
            }
        }
        public string Key
        {
            get
            {
                return m_key;
            }

            set
            {
                m_key = value;
            }
        }
        public DateTime Created
        {
            get
            {
                return m_created;
            }

            set
            {
                m_created = value;
            }
        }
        public int UserID
        {
            get
            {
                return m_userID;
            }

            set
            {
                m_userID = value;
            }
        }

        public int ErrorCode
        {
            get
            {
                return m_errorCode;
            }

            set
            {
                m_errorCode = value;
            }
        }

        public AuthenticationToken()
        {

        }

        private AuthenticationToken(DateTime expiry,string key,DateTime created,int userID)
        {
            m_expiry = expiry;
            m_key = key;
            m_created = created;
            m_userID = userID;
        }

        public static AuthenticationToken CreateAuthenticationToken(int m_userID)
        {
            return new AuthenticationToken(DateTime.UtcNow.AddMinutes(5), SecurityHelper.CreateKey(32), DateTime.UtcNow, m_userID);
        }

        private static AuthenticationToken CreateInfiniteAuthenticationToken()
        {
            AuthenticationToken retVal = CreateAuthenticationToken(-1);
            retVal.Expiry = DateTime.MinValue;
            return retVal;
        }
        protected override void LoadFrom(IDataAdapter adapter)
        {
            m_expiry = adapter.GetValue("Expires", DateTime.UtcNow);
            m_key = adapter.GetValue("Key", string.Empty);
            m_created = adapter.GetValue("Created", DateTime.UtcNow);
            m_userID = adapter.GetValue("UserID", 0);
            m_errorCode = adapter.GetValue("ErrorCode", (int)eTokenError.Error);
        }
    }
}
