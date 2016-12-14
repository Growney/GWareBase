using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public struct APIConnectionStatus
    {
        private bool m_databaseExists;
        private bool m_webserviceExists;
        private bool m_databaseConnected;

        public bool DatabaseExists
        {
            get
            {
                return m_databaseExists;
            }

            set
            {
                m_databaseExists = value;
            }
        }

        public bool WebserviceExists
        {
            get
            {
                return m_webserviceExists;
            }

            set
            {
                m_webserviceExists = value;
            }
        }

        public bool DatabaseConnected
        {
            get
            {
                return m_databaseConnected;
            }

            set
            {
                m_databaseConnected = value;
            }
        }

        public APIConnectionStatus(bool databaseExists,bool webserviceExists,bool databaseConnected)
        {
            m_databaseExists = databaseExists;
            m_webserviceExists = webserviceExists;
            m_databaseConnected = databaseConnected;
        }

        public void Merge(APIConnectionStatus status)
        {
            m_databaseConnected |= status.DatabaseConnected;
            m_webserviceExists |= status.WebserviceExists;
            m_databaseExists |= status.DatabaseExists;
        }
    }
}
