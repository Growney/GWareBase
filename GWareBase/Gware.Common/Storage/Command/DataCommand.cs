using Gware.Common.Security.Authentication;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.DataStructures;
using System.Collections;

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
        private Dictionary<string,IDataCommandParameter> m_parameters = new Dictionary<string, IDataCommandParameter>();


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

        public IDataCommandParameter this[string name]
        {
            get
            {
                return GetParameter(name);
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
            string nameLower = param.Name.ToLower();
            if (GetParameter(nameLower) == null)
            {
                m_parameters.Set(nameLower, param);
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
        public IDataCommandParameter AddParameter(string name,string datatypeName)
        {
            return AddParameter(new DataCommandParameter(name, null, DbType.Object, ParameterDirection.Input));
        }



        public object GetParameterValue(string name)
        {
            return GetParameter(name).Value;
        }

        public IDataCommandParameter GetParameter(string name)
        {
            return m_parameters.Get(name);
        }
        
        public void SetParameter(string name, object value)
        {
            GetParameter(name).Value = value;
        }
     
        public void AddReCacheCommand(IDataCommand command)
        {
            m_recacheCommands.Add(command);
        }

        public void AddReCacheCommand(ICollection<IDataCommand> commands)
        {
            m_recacheCommands.AddRange(commands);
        }

        public IEnumerator<IDataCommandParameter> GetEnumerator()
        {
            foreach(string key in m_parameters.Keys)
            {
                yield return m_parameters[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
