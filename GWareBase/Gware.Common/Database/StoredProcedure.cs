using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Gware.Common.Database
{
    public class StoredProcedure
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
        
        public void AddParameter(string name, SqlDbType type, ParameterDirection direction)
        {
            if (!m_parameters.ContainsKey(name))
            {
                SqlParameter param = new SqlParameter(name, type);
                param.Direction = direction;
                m_parameters.Add(name, param);
            }
        }
        public void AddParameter(string name, SqlDbType type)
        {
            AddParameter(name, type, ParameterDirection.Input);
        }
        public SqlParameter GetParameter(string name)
        {
            if (m_parameters.ContainsKey(name))
            {
                return m_parameters[name];
            }
            return null;
        }
        public void SetParameterValue(string name, object value)
        {
            if (m_parameters.ContainsKey(name))
            {
                SqlParameter param = m_parameters[name];
                param.Value = value;
            }
        }
    }
}
