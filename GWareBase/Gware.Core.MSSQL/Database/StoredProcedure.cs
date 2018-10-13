using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

namespace Gware.Core.MSSQL.Database
{
    public class StoredProcedure
    {
        private Dictionary<string, SqlParameter> m_parameters;

        internal IEnumerable<SqlParameter> Parameters
        {
            get { return m_parameters.Values; }
        }

        public string Name { get; }

        public StoredProcedure(string name)
        {
            Name = name;
            m_parameters = new Dictionary<string, SqlParameter>();
            OnInitialiseParameters();
        }
        protected virtual void OnInitialiseParameters()
        {

        }
        public void AddParameter(string name, SqlDbType type, string dataTypeName, ParameterDirection direction)
        {
            if (!m_parameters.ContainsKey(name))
            {
                SqlParameter param = new SqlParameter(name, type);
                param.Direction = direction;
                param.TypeName = dataTypeName;
                m_parameters.Add(name, param);
            }
        }
        public void AddParameter(string name, SqlDbType type)
        {
            AddParameter(name, type, string.Empty, ParameterDirection.Input);
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
                switch (value)
                {
                    case DateTime x when value is DateTime:
                        {
                            if (x < SqlDateTime.MinValue.Value)
                            {
                                value = SqlDateTime.MinValue.Value;
                            }
                            else if (x > SqlDateTime.MaxValue.Value)
                            {
                                value = SqlDateTime.MaxValue.Value;
                            }
                        }
                        break;
                }
                param.Value = value;
            }
        }

        public string GetParameterErrorString()
        {
            StringBuilder retVal = new StringBuilder();

            foreach(SqlParameter parameter in m_parameters.Values)
            {
                retVal.AppendLine($"{parameter.ParameterName} - {parameter.Value}");
            }

            return retVal.ToString();
        }
    }
}
