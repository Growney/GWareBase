using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public struct LoginResult
    {
        private string m_username;

        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }
        private string m_password;

        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }
        private bool m_rememberMe;

        public bool RememberMe
        {
            get { return m_rememberMe; }
            set { m_rememberMe = value; }
        }

        private bool m_cancel;

        public bool Cancel
        {
            get { return m_cancel; }
            set { m_cancel = value; }
        }

        public LoginResult(string username, string password)
        {
            m_username = username;
            m_password = password;
            m_rememberMe = false;
            m_cancel = false;
        }

        public LoginResult(string username, string password, bool rememberMe)
        {
            m_username = username;
            m_password = password;
            m_rememberMe = rememberMe;
            m_cancel = false;
        }
        public LoginResult(bool cancel)
        {
            m_username = string.Empty;
            m_password = string.Empty;
            m_rememberMe = false;
            m_cancel = cancel;
        }
    }
}
