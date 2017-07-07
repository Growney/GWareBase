using Gware.Common.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command
{
    public abstract class DataCommandFactory
    {
        private static AuthenticationToken m_token;
        public static AuthenticationToken Token
        {
            get
            {
                return m_token;
            }
            set
            {
                m_token = value;
            }
        }
        private bool m_cache;
        private bool m_requiredAuthentication;
        private string m_commandName;

        public DataCommandFactory(string commandName, bool requiredAuthentication,bool cache)
        {
            m_cache = cache;
            m_commandName = commandName;
            m_requiredAuthentication = requiredAuthentication;
        }

        public DataCommandFactory(string commandName,bool requiredAuthentication)
            :this(commandName,requiredAuthentication,true)
        {
            
        }
        public DataCommandFactory(string commandName)
            :this(commandName,false)
        {
        }

        protected DataCommand CreateCommand(string method, params IDataCommand[] recacheCommands)
        {
            return CreateCommand(method, m_requiredAuthentication,m_cache,recacheCommands);
        }

        protected DataCommand CreateCommand(string method)
        {
            return CreateCommand(method,m_requiredAuthentication);
        }

        protected DataCommand CreateCommand(string method,bool requiredAuthentication)
        {
            return CreateCommand(method, requiredAuthentication, true, new IDataCommand[0]);
        }
        protected DataCommand CreateCommand(string method, bool requiredAuthentication,bool cache)
        {
            return CreateCommand(method, requiredAuthentication, cache, new IDataCommand[0]);
        }
        protected DataCommand CreateCommand(string method, bool requiredAuthentication,bool cache,params IDataCommand[] recacheCommands)
        {
            DataCommand retVal = new DataCommand(m_commandName, method, (requiredAuthentication) ? m_token : null, cache, recacheCommands);

            return retVal;
        }
    }
}
