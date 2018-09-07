using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Controller
{
    public class DelegatedControllerProvider : IControllerProvider
    {
        private readonly Func<string, ICommandController> m_func;
        public DelegatedControllerProvider(Func<string,ICommandController> func)
        {
            m_func = func;
        }
        public ICommandController CreateController(string key)
        {
            return m_func(key);
        }
    }
}
