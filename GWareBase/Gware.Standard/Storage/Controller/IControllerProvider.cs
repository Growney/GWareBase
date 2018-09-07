using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Controller
{
    public interface IControllerProvider
    {
        ICommandController CreateController(string key);
    }
}
