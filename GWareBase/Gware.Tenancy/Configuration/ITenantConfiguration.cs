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
        IActionResult Upgrading { get; }
        ICommandController Controller { get; }
        String SchemaFile { get; }
        String DBNameFormat { get; }
        String ControllerKey { get; }
        string[] Domains { get; }
        bool IgnorePorts { get; }

        Task<bool> CreateTenant(string name,string displayName, int entityType, long entityID);
        Task<bool> UpgradeTenant(Tenant tenant, DateTime check);
        string GetTenantRedirect(string tenantName, string path);
        string GetTenantRedirect(int entityType, long entityID,string path);
        bool DoesTenantExist(string name);
        bool DoesTenantExist(int entityType, long entityID);

        bool IsValidTenantName(string name);

        DateTime GetSchemaCreated();

        Tenant GetTenant(int entityType, long entityID);

        void CreateTenantLink(long tenantID, byte type, string link);
        void DeleteTenantLink(long tenantID, byte type);
        Tenant GetTenantFromLink(string link);
        string GetLink(long tenantID, byte type);
        
    }
}
