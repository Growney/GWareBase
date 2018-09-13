
using Gware.Standard.Storage.Controller;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public interface ITenantControllerProvider
    {
        ICommandController GetDefaultDataController();
        ICommandController GetController();
        ICommandController GetController(HttpContext context);
    }
}
