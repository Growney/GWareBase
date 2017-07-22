using Gware.Common.Security.Authentication;
using Gware.Common.Storage.Command.Interface;
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
        private bool m_cache;
        private List<IDataCommand> m_recacheCommands = new List<IDataCommand>();
        private List<IDataCommandParameter> m_parameters = new List<IDataCommandParameter>();


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

        public bool TriggersReCache
        {
            get
            {
                return ReCacheCommands.Count > 0;
            }
        }

        public bool Cache
        {
            get
            {
                return m_cache;
            }
            set
            {
                m_cache = value;
            }
        }

        public List<IDataCommand> ReCacheCommands
        {
            get
            {
                return m_recacheCommands;
            }
        }

        public DataCommand(string commandName,string commandMethod)
            :this(commandName,commandMethod,null)
        {
        }
        public DataCommand(string commandName, string commandMethod,AuthenticationToken token)
            :this(commandName,commandMethod,token,true)
        {
        }
        public DataCommand(string commandName, string commandMethod, AuthenticationToken token,bool cache)
            :this(commandName, commandMethod, token, true,new IDataCommand[0])
        {
            
        }
        public DataCommand(string commandName, string commandMethod, AuthenticationToken token, bool cache,params IDataCommand[] recacheCommands)
        {
            m_commandName = commandName;
            m_commandMethod = commandMethod;
            m_token = token;
            m_cache = cache;

            if(recacheCommands.Length > 0)
            {
                m_recacheCommands.AddRange(recacheCommands);
            }
        }
        public IDataCommandParameter AddParameter(string name, DbType dataType)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType));
        }
        public IDataCommandParameter AddParameter(IDataCommandParameter param)
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
        public IDataCommandParameter AddParameter(string name, DbType dataType, ParameterDirection direction)
        {
            return AddParameter(new DataCommandParameter(name,  null, dataType, direction));
        }

        public IDataCommandParameter AddParameter(string name, DbType dataType, ParameterDirection direction, bool anyValueInCache)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType, direction,anyValueInCache));
        }

        public IDataCommandParameter AddParameter(string name, DbType dataType, bool anyValueInCache)
        {
            return AddParameter(new DataCommandParameter(name, null, dataType, anyValueInCache));
        }

        public object GetParameterValue(int index)
        {
            return GetParameter(index).Value;
        }

        public object GetParameterValue(string name)
        {
            return GetParameter(name).Value;
        }

        public IDataCommandParameter GetParameter(string name)
        {
            IDataCommandParameter retVal = null;
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

        public IDataCommandParameter GetParameter(int index)
        {
            return m_parameters[index];
        }

        public void SetParameter(string name, object value)
        {
            GetParameter(name).Value = value;
        }

        public override bool Equals(object obj)
        {
            if(obj is DataCommand)
            {
                return Equals(obj as DataCommand);
            }
            return base.Equals(obj);
        }

        public bool Equals(DataCommand val)
        {
            bool retVal = Name.Equals(val.Name) && CommandMethod.Equals(val.CommandMethod);
            if (retVal)
            {
                retVal = m_parameters.Count == val.m_parameters.Count;
                if (retVal)
                {
                    for (int i = 0; i < m_parameters.Count; i++)
                    {
                        retVal = m_parameters[i].Equals(val.m_parameters[i]);
                        if (!retVal)
                        {
                            break;
                        }
                    }
                }
            }
            return retVal;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool cache)
        {
            StringBuilder cacheString = new StringBuilder();
            cacheString.AppendFormat("{0}:{1}?", Name, CommandMethod);
            for (int i = 0; i < m_parameters.Count; i++)
            {
                IDataCommandParameter param = m_parameters[i];
                cacheString.Append(param.ToString(cache));
            }
            return cacheString.ToString();
        }

        public override int GetHashCode()
        {
            return GetHashCode(false);
        }

        public int GetHashCode(bool cache)
        {
            return ToString(cache).GetHashCode();
        }

        public void AddReCacheCommand(IDataCommand command)
        {
            m_recacheCommands.Add(command);
        }

        public void AddReCacheCommand(ICollection<IDataCommand> commands)
        {
            m_recacheCommands.AddRange(commands);
        }

        
    }
}
