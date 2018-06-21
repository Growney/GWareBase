using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gware.Common.Context
{
    public interface IControllerProvider
    {
        ICommandController CreateController(string key);
    }
}
