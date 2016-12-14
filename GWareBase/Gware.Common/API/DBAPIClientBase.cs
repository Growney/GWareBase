using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public abstract class DBAPIClientBase : APIClientBase
    {
        private string m_serverName;
        private string m_databaseName;
        private string m_databaseUsername;
        private string m_databasePassword;
        public string ServerName
        {
            get
            {
                return m_serverName;
            }

            set
            {
                m_serverName = value;
            }
        }
        public string DatabaseName
        {
            get
            {
                return m_databaseName;
            }

            set
            {
                m_databaseName = value;
            }
        }
        public string DatabaseUsername
        {
            get
            {
                return m_databaseUsername;
            }

            set
            {
                m_databaseUsername = value;
            }
        }
        public string DatabasePassword
        {
            get
            {
                return m_databasePassword;
            }

            set
            {
                m_databasePassword = value;
            }
        }
        public DBAPIClientBase(string serverName, string databaseName, string databaseUsername, string databasePassword)
            :base()
        {
            m_serverName = serverName;
            m_databaseName = databaseName;
            m_databaseUsername = databaseUsername;
            m_databasePassword = databasePassword;
        }
        public DBAPIClientBase(string serverName,string databaseName,string databaseUsername, string databasePassword,string username, string password) 
            : base(username, password)
        {
            m_serverName = serverName;
            m_databaseName = databaseName;
            m_databaseUsername = databaseUsername;
            m_databasePassword = databasePassword;
        }
        

    }
}
