using Gware.Common.API.Session;
using Gware.Common.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API.Database
{
    public abstract class MSSQLDBClientBase : DBAPIClientBase
    {
        private MSSQLDBConnection m_connection;
        private MSSQLDBConnection m_masterConnection;

        public MSSQLDBConnection Connection
        {
            get
            {
                return new MSSQLDBConnection("MSSQLDBClient", ServerName, DatabaseName, DatabaseUsername, DatabasePassword);
            }
        }
        public MSSQLDBConnection MasterConnection
        {
            get
            {
                return new MSSQLDBConnection("MSSQLDBClient", ServerName, "master", DatabaseUsername, DatabasePassword); ;
            }
        }
        public MSSQLDBClientBase(ISessonManager sessonManager,string serverName, string databaseName, string databaseUsername, string databasePassword) 
            : base(sessonManager,serverName, databaseName, databaseUsername, databasePassword)
        {
            
        }

        public override bool CanConnect()
        {
            bool retVal = true;
            try
            {
                MSSQLDBConnection masterSettings = MasterConnection;
                masterSettings.TimeOut = 5;
                using (SqlConnection conn = masterSettings.GetConnection())
                {
                    conn.Open();
                }
            }
            catch
            {
                retVal = false;
            }
            return retVal;
        }
        public bool DatabaseExists()
        {
            bool retVal = false;

            if (CanConnect())
            {
                MasterConnection.ExecuteQuery("SELECT name FROM sys.databases WHERE name = N'{0}'", DatabaseName);
            }

            return retVal;
        }

        public override APIConnectionStatus GetConnectionStatus()
        {
            bool serverExists = CanConnect();
            bool databaseExists = serverExists && DatabaseExists();
            APIConnectionStatus retVal = new APIConnectionStatus(databaseExists, false, serverExists);
            return retVal;
        }

    }
}
