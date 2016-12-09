using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Gware.Common.Database
{
    public abstract class StoredProcedure
    {
        private string m_name;
        private Dictionary<string, SqlParameter> m_parameters;

        internal IEnumerable<SqlParameter> Parameters
        {
            get { return m_parameters.Values; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public StoredProcedure(string name)
        {
            m_name = name;
            m_parameters = new Dictionary<string,SqlParameter>();
            OnInitialiseParameters();
        }
        protected virtual void OnInitialiseParameters()
        {

        }
        protected void AddParameter(string name, SqlDbType type, ParameterDirection direction)
        {
            if (!m_parameters.ContainsKey(name))
            {
                SqlParameter param = new SqlParameter(name, type);
                param.Direction = direction;
                m_parameters.Add(name, param);
            }
        }
        protected void AddParameter(string name, SqlDbType type)
        {
            AddParameter(name, type, ParameterDirection.Input);
        }
        protected SqlParameter GetParameter(string name)
        {
            if (m_parameters.ContainsKey(name))
            {
                return m_parameters[name];
            }
            return null;
        }
        protected void SetParameterValue(string name, object value)
        {
            if (m_parameters.ContainsKey(name))
            {
                SqlParameter param = m_parameters[name];
                param.Value = value;
            }
        }
        protected int ExecuteNonQuery(DatabaseConnection conn)
        {
            return conn.ExecuteNonQuery(this);
        }
        protected int ExecuteNonQuery(string connectionName)
        {
            DatabaseConnection connection = Gware.Common.Application.ApplicationBase.GetConnection(connectionName);
            if (connection != null)
            {
                return ExecuteNonQuery(connection);
            }
            return -1;
        }
        protected DataSet ExecuteQuery(string connectionName)
        {
            DatabaseConnection connection = Gware.Common.Application.ApplicationBase.GetConnection(connectionName);
            if (connection != null)
            {
                return ExecuteQuery(connection);
            }
            return null;
        }
        protected DataSet ExecuteQuery(DatabaseConnection connection)
        {
            return connection.ExecuteQuery(this);
        }

    }
}
