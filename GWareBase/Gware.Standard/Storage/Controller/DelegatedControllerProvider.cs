using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Controller
{
    public class DelegatedControllerProvider<T> : IControllerProvider where T : ICommandController
    {
        private readonly Func<ILogger<T>, string, T> m_func;
        private readonly string m_defaultKey;
        private readonly ILogger<T> m_logger;
        public DelegatedControllerProvider(ILogger<T> logger,Func<ILogger<T>,string, T> func,string defaultKey = "Default")
        {
            m_logger = logger;
            m_func = func;
            m_defaultKey = defaultKey;
        }
        public ICommandController CreateController(string key)
        {
            return m_func(m_logger,key);
        }

        public ICommandController GetDefaultDataController()
        {
            return m_func(m_logger,m_defaultKey);
        }
    }
}
