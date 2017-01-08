using System.Data;
using System.Data.SqlClient;

namespace Gware.Common.Database
{
    public interface IDBConnection
    {
        string ConnectionName { get; set; }
        string DatabaseName { get; set; }
        bool Encrypted { get; set; }
        string Password { get; set; }
        string ServerName { get; set; }
        int TimeOut { get; set; }
        string Username { get; set; }

        int ExecuteNonQuery(string query);
        int ExecuteNonQuery(StoredProcedure sp);
        int ExecuteNonQuery(string queryFormat, params string[] parameters);
        DataSet ExecuteQuery(string query);
        DataSet ExecuteQuery(StoredProcedure sp);
        DataSet ExecuteQuery(string queryFormat, params string[] parameters);
        SqlConnection GetConnection();
    }
}