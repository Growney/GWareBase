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
        private bool m_requiredAuthentication;
        private string m_commandName;

        public DataCommandFactory(string commandName,bool requiredAuthentication)
        {
            m_commandName = commandName;
            m_requiredAuthentication = requiredAuthentication;
        }
        public DataCommandFactory(string commandName)
            :this(commandName,false)
        {
        }

        protected DataCommand CreateCommand(string method)
        {
            return CreateCommand(method,m_requiredAuthentication);
        }

        protected DataCommand CreateCommand(string method,bool requiredAuthentication)
        {
            DataCommand retVal;
            if (requiredAuthentication)
            {
                retVal = new DataCommand(m_commandName, method,m_token);
            }
            else
            {
                retVal = new DataCommand(m_commandName, method);
            }
            return retVal;
        }
    }
}
