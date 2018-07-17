using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Configuration
{
    public interface ITenantControllerProvider
    {
        ICommandController GetController();
    }
}
