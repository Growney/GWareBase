using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Gware.Common.XML;
using Gware.Common.Encryption;

namespace Gware.Common.Database
{
    public class MSSQLDBConnection : IDBConnection
    {
        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionName { get; set; }
        public bool Trusted { get; set; }

        public int TimeOut { get; set; } = 30;

        public bool Encrypted { get; set; }

        public MSSQLDBConnection(string connectionName, string serverName, string databaseName,bool trusted, string username, string password)
        {
            ConnectionName = connectionName;
            ServerName = serverName;
            DatabaseName = databaseName;
            Trusted = trusted;
            Username = username;
            Password = password;
        }

        public MSSQLDBConnection(XmlNode node)
            : this(node.GetAttributeString("ConnectionName", string.Empty), 
            node.GetString("DatabaseName", string.Empty), 
            node.GetString("ServerName", string.Empty),
            node.GetBoolean("Trusted",false),
            node.GetString("Username", string.Empty), 
            node.GetString("Password", string.Empty))
        {
           
        }
        public void AppendTo(XmlNode node)
        {
            node.SetAttribute("ConnectionName", ConnectionName);

            node.Set("DatabaseName", DatabaseName);
            node.Set("ServerName", ServerName);
            node.Set("Username", Username);
            node.Set("Password", Password);
            
        }
        public string GetConnectionString()
        {
            if (Trusted)
            {
                return $"Server={ServerName};Database={DatabaseName};Trusted_Connection=True;Encrypt={Encrypted};Connection Timeout={TimeOut};";
            }
            else
            {
                return $"Server={ServerName};Database={DatabaseName};User id={Username};Password={Password};Trusted_Connection=False;Encrypt={Encrypted};Connection Timeout={TimeOut};";
            }
            
        }
        public SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();
            return new SqlConnection(connectionString);
        }
        public int ExecuteNonQuery(StoredProcedure sp)
        {
            
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    comm.CommandText = sp.Name;
                    foreach (SqlParameter param in sp.Parameters)
                    {
                        comm.Parameters.Add(param);
                    }

                    return comm.ExecuteNonQuery();
                }
            }
        }
        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = System.Data.CommandType.Text;
                    comm.CommandText = query;
                    return comm.ExecuteNonQuery();
                }
            }
        }
        public int ExecuteNonQuery(string queryFormat, params string[] parameters)
        {
            return ExecuteNonQuery(string.Format(queryFormat, parameters));
        }
        public DataSet ExecuteQuery(string query)
        {
            DataSet retVal = new DataSet();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = query;
                    using (SqlDataAdapter reader = new SqlDataAdapter(comm))
                    {
                        reader.Fill(retVal);
                    }
                }
            }
            return retVal;
        }
        public DataTable ExecuteTableQuery(string query)
        {
            return GetDataTable(ExecuteQuery(query));   
        }
        public DataTable ExecuteTableQuery(StoredProcedure sp)
        {
            return GetDataTable(ExecuteQuery(sp));
        }
        private DataTable GetDataTable(DataSet data)
        {
            DataTable retVal = null;
            if (data != null)
            {
                if (data.Tables.Count > 0)
                {
                    retVal = data.Tables[0];
                }
            }
            return retVal;
        }
        public DataSet ExecuteQuery(StoredProcedure sp)
        {
            DataSet retVal = new DataSet();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = sp.Name;
                    foreach (SqlParameter param in sp.Parameters)
                    {
                        comm.Parameters.Add(param);
                    }
                    using (SqlDataAdapter reader = new SqlDataAdapter(comm))
                    {
                        reader.Fill(retVal);
                    }
                }
            }
            return retVal;
        }
        public DataSet ExecuteQuery(string queryFormat,params string[] parameters)
        {
            return ExecuteQuery(string.Format(queryFormat, parameters));
        }

        

    }
}
