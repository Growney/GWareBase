using Gware.Common.Storage.Command.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Configuration
{
    public interface ITenantControllerProvider
    {
        ICommandController GetController();
        ICommandController GetController(HttpContext context);
    }
}
