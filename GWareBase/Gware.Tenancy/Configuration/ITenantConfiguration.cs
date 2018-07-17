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
        IActionResult CreateNewResult { get; }
        IActionResult NotFoundResult { get; }
        IActionResult TenantHome { get; }
        ICommandController Controller { get; }
        String SchemaFile { get; }
        String DBNameFormat { get; }
        String ControllerKey { get; }
        string[] Domains { get; }

        Task<bool> CreateTenant(string name,string displayName,string imagesource,bool includeComposite = false);
        string GetTenantRedirect(string tenantName, string path);
        bool DoesTenantExists(string name);
        bool IsValidTenantName(string name);
    }
}
