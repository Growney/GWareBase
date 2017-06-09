using Gware.Common.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command
{
    public class DataCommand : IDataCommand
    {
        private string m_commandName;
        private string m_commandMethod;
        private Exception m_exception;
        private bool m_success;
        private AuthenticationToken m_token;

        private List<DataCommandParameter> m_parameters = new List<DataCommandParameter>();
        

        public string CommandMethod
        {
            get
            {
                return m_commandMethod;
            }
        }
        public string Name
        {
            get
            {
                return m_commandName;   
            }
        }
        public Exception Exception
        {
            get
            {
                return m_exception;
            }
            set
            {
                m_exception = value;
            }
        }

        public bool Success
        {
            get
            {
                return m_success;
            }
            set
            {
                m_success = value;
            }
        }
        public int ParameterCount
        {
            get
            {
                return m_parameters.Count;
            }
        }

        public DataCommand(string commandName,string commandMethod)
        {
            m_commandName = commandName;
            m_commandMethod = commandMethod;
        }
        public DataCommand(string commandName, string commandMethod,AuthenticationToken token)
        {
            m_commandName = commandName;
            m_commandMethod = commandMethod;
            m_token = token;
        }


        public DataCommandParameter AddParameter(string name, DbType dataType)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType));
        }
        public DataCommandParameter AddParameter(DataCommandParameter param)
        {
            if (GetParameter(param.Name) == null)
            {
                m_parameters.Add(param);
                return param;
            }
            else
            {
                throw new ArgumentException("An element with the same key already exists in the collection");
            }
        }
        public DataCommandParameter AddParameter(string name, DbType dataType, ParameterDirection direction)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType, direction));
        }
        
        public object GetParameterValue(int index)
        {
            return GetParameter(index).Value;
        }

        public object GetParameterValue(string name)
        {
            return GetParameter(name).Value;
        }

        public DataCommandParameter GetParameter(string name)
        {
            DataCommandParameter retVal = null;
            string loweredName = name.ToLower();
            for (int i = 0; i < m_parameters.Count; i++)
            {
                if (m_parameters[i].Name.ToLower() == loweredName)
                {
                    retVal = m_parameters[i];
                    break;
                }
            }

            return retVal;
        }

        public DataCommandParameter GetParameter(int index)
        {
            return m_parameters[index];
        }

        public void SetParameter(string name, object value)
        {
            GetParameter(name).Value = value;
        }
    }
}
