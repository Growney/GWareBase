using Gware.Common.Storage.Command.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Tenancy.Configuration
{
    public interface ITenantConfiguration
    {
        IActionResult NotFoundResult { get; }
        IActionResult NotExistsResult { get; }
        ICommandController Controller { get; }
        String SchemaFile { get; }
        String DBNameFormat { get; }

        Task<bool> CreateTenant(string name);
    }
}
