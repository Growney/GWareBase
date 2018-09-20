using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public interface ITenantConfiguration
    {
        IActionResult CreateNewResult { get; }
        IActionResult NotFoundResult { get; }
        IActionResult TenantHome { get; }
        IActionResult Upgrading { get; }
        ICommandController Controller { get; }
        string SchemaFile { get; }
        string DBNameFormat { get; }
        string ControllerKey { get; }
        RouteTemplateDomain[] Domains { get; }

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

        ICommandController GetTenantController(Tenant tenant);

    }
}
