using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Controller
{
    public class DelegatedControllerProvider : IControllerProvider
    {
        private readonly Func<string, ICommandController> m_func;
        private readonly string m_defaultKey;
        public DelegatedControllerProvider(Func<string,ICommandController> func,string defaultKey = "Default")
        {
            m_func = func;
            m_defaultKey = defaultKey;
        }
        public ICommandController CreateController(string key)
        {
            return m_func(key);
        }

        public ICommandController GetDefaultDataController()
        {
            return m_func(m_defaultKey);
        }
    }
}
