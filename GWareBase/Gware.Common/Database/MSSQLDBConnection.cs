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
        private string m_connectionName;
        private string m_databaseName;
        private string m_password;
        private string m_username;
        private string m_serverName;
        private int m_timeOut = 30;
        private bool m_encrypted;
        
        public string DatabaseName
        {
            get { return m_databaseName; }
            set { m_databaseName = value; }
        }
        public string ServerName
        {
            get { return m_serverName; }
            set { m_serverName = value; }
        }
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        public string ConnectionName
        {
            get { return m_connectionName; }
            set { m_connectionName = value; }
        }

        public int TimeOut
        {
            get
            {
                return m_timeOut;
            }

            set
            {
                m_timeOut = value;
            }
        }

        public bool Encrypted
        {
            get
            {
                return m_encrypted;
            }

            set
            {
                m_encrypted = value;
            }
        }

        public MSSQLDBConnection(string connectionName, string serverName, string databaseName, string username, string password)
        {
            ConnectionName = connectionName;
            ServerName = serverName;
            DatabaseName = databaseName;
            Username = username;
            Password = password;
            
        }

        public MSSQLDBConnection(XmlNode node)
            : this(node.GetAttributeString("ConnectionName", string.Empty), 
            node.GetString("DatabaseName", string.Empty), 
            node.GetString("ServerName", string.Empty), 
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
        private string GetApplicationConnectionString()
        {
            return string.Format("Server={0};Database={1};User id={2};Password={3};Trusted_Connection=False;Encrypt={5};Connection Timeout={4};", m_serverName, m_databaseName, m_username, m_password,m_timeOut,m_encrypted);
        }
        public SqlConnection GetConnection()
        {
            string connectionString = GetApplicationConnectionString();
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
