using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public enum AuthenticationErrorCodes
    {
        None = 0,
        UsernameNotFound = 1,
        PasswordDoesNotMatch = 2,
        SubscriptionExpired = 3,
        SystemError = 4,
    }
    public struct UserAuthenticationResult
    {
        private string m_username;
        private bool m_success;
        private int m_errorCode;

        private UserAuthenticationKey m_key;

        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }
        public bool Success
        {
            get { return m_success; }
            set { m_success = value; }
        }
        public int ErrorCode
        {
            get { return m_errorCode; }
            set { m_errorCode = value; }
        }
        public UserAuthenticationKey Key
        {
            get { return m_key; }
            set { m_key = value; }
        }
    }
}
