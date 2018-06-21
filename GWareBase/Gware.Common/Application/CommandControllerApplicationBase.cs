using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Reflection;
using Gware.Common.DataStructures;
using Gware.Common.Context;

namespace Gware.Common.Application
{
    public static class CommandControllerApplicationBase
    {
        public static ApplicationBase AppBase { get; private set; }
        
        public static void InitialiseBase(ApplicationBase appBase)
        {
            AppBase = appBase;
        }
    }
}
