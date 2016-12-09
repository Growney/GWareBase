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
    public class DatabaseConnection
    {
        private string m_connectionName;
        private string m_databaseName;
        private string m_password;
        private string m_username;
        private string m_serverName;
        private bool m_encrypted;

        public bool Encrypted
        {
            get { return m_encrypted; }
            set { m_encrypted = value; }
        }
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

        public DatabaseConnection(string connectionName, string serverName, string databaseName, string username, string password,bool encrypted)
        {
            ConnectionName = connectionName;
            if (encrypted)
            {
                ServerName = SimpleAES.DecryptValue(serverName);
                DatabaseName = SimpleAES.DecryptValue(databaseName);
                Username = SimpleAES.DecryptValue(username);
                Password = SimpleAES.DecryptValue(password);
            }
            else
            {
                ServerName = serverName;
                DatabaseName = databaseName;
                Username = username;
                Password = password;
            }
            
        }

        public DatabaseConnection(XmlNode node)
            : this(node.GetAttributeString("ConnectionName", string.Empty), 
            node.GetString("DatabaseName", string.Empty), 
            node.GetString("ServerName", string.Empty), 
            node.GetString("Username", string.Empty), 
            node.GetString("Password", string.Empty),
            node.GetBoolean("Encrypted",true))
        {
           
        }
        public void AppendTo(XmlNode node)
        {
            node.SetAttribute("ConnectionName", SimpleAES.EncryptValue(ConnectionName));
            node.Set("Encrypted", Encrypted);
            if (Encrypted)
            {
                node.Set("DatabaseName", SimpleAES.EncryptValue(DatabaseName));
                node.Set("ServerName", SimpleAES.EncryptValue(ServerName));
                node.Set("Username", SimpleAES.EncryptValue(Username));
                node.Set("Password", SimpleAES.EncryptValue(Password));
            }
            else
            {
                node.Set("DatabaseName", DatabaseName);
                node.Set("ServerName", ServerName);
                node.Set("Username", Username);
                node.Set("Password", Password);
            }
            
        }
        private string GetApplicationConnectionString()
        {
            return string.Format("Server={0};Database={1};User id={2};Password={3};Trusted_Connection=False;Encrypt=True;Connection Timeout=30;", m_serverName, m_databaseName, m_username, m_password);
        }

        public int ExecuteNonQuery(StoredProcedure sp)
        {
            string connectionString = GetApplicationConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(GetApplicationConnectionString()))
            {
                using (SqlCommand comm = conn.CreateCommand())
                {
                    comm.CommandType = System.Data.CommandType.Text;
                    comm.CommandText = query;
                    return comm.ExecuteNonQuery();
                }
            }
        }
        public DataSet ExecuteQuery(string query)
        {
            DataSet retVal = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetApplicationConnectionString()))
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
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        public DataSet ExecuteQuery(StoredProcedure sp)
        {
            DataSet retVal = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetApplicationConnectionString()))
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
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }


    }
}
