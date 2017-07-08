using Gware.Common.Database;
using System.Data.SqlClient;

namespace Gware.Common.Client.Database
{
    public abstract class MSSQLDBClientBase : DBAPIClientBase
    {

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

        public MSSQLDBClientBase(string serverName, string databaseName, string databaseUsername, string databasePassword) 
            : base(serverName, databaseName, databaseUsername, databasePassword)
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
        public override ClientConnectionStatus GetConnectionStatus()
        {
            bool serverExists = CanConnect();
            bool databaseExists = serverExists && DatabaseExists();
            ClientConnectionStatus retVal = new ClientConnectionStatus(databaseExists,1);
            return retVal;
        }
    }
}
