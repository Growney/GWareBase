using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public interface ITenantConfiguration
    {
        ICommandController Controller { get; set; }
        string SchemaFile { get; set; }
        string DBNameFormat { get; set; }
        RouteTemplateDomain[] Domains { get; set; }
        bool CreateComposite { get; set; }
        Func<ICommandController, Task<bool>> OnDeployTenantSchema { get; set; }

        Task<bool> UpgradeTenant(Tenant tenant, DateTime check);
        string GetTenantRedirect(string tenantName, string path);
        string GetTenantRedirect(int entityType, long entityID,string path);
        bool DoesTenantExist(string name);
        bool DoesTenantExist(int entityType, long entityID);

        DateTime GetSchemaCreated();

        Tenant GetTenant(long tenantID);
        Tenant GetTenant(int entityType, long entityID);
        string GetLink(long tenantID, byte type);

        void CreateTenantLink(long tenantID, byte type, string link);
        void DeleteTenantLink(long tenantID, byte type);
        LinkedTenant GetTenantFromLink(string link);
        List<LinkedTenant> AllWithLink(byte type);

        ICommandController GetTenantController(Tenant tenant);

    }
}
