using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public class TenantConfiguration : ITenantConfiguration
    {
        public IActionResult Upgrading { get; set; }
        public IActionResult NotFoundResult { get; set; }
        public IActionResult TenantHome { get; set; }
        public IActionResult CreateNewResult { get; set; }
        public ICommandController Controller { get; set; }
        public string SchemaFile { get; set; }
        public string DBNameFormat { get; set; }
        public string ControllerKey { get; set; }
        public RouteTemplateDomain[] Domains { get; set; }
        public bool CreateComposite { get; set; }
        public Func<ICommandController,Task<bool>> OnDeployTenantSchema { get; set; }
        public Assembly[] SearchIn { get; set; }

        public TenantConfiguration()
        {
            ControllerKey = "tenant";
        }

        public ICommandController GetTenantController(Tenant tenant)
        {
            return tenant?.GetController(SearchIn);
        }

        public Task<bool> CreateTenant(string name,string displayName, int entityType, long entityID)
        {
            if (Tenant.IsValidTenantName(name))
            {
                if (!Tenant.Exists(Controller, name))
                {
                    ICommandController newTenancyController = Controller.Clone();
                    newTenancyController.SetName(string.Format(DBNameFormat, name));
                    Tenant.Create(Controller, newTenancyController, name, displayName,entityType,entityID,GetSchemaCreated());

                    return OnDeployTenantSchema(newTenancyController);
                }
            }
            return Task.FromResult(false);
        }
        public string GetDBName(string tenantName)
        {
            return string.Format(DBNameFormat,tenantName);
        }
        public async Task<bool> UpgradeTenant(Tenant tenant,DateTime check)
        {
            eUpgradeStatus status = tenant.CheckUpgradeStatus(Controller, check);
            if(status == eUpgradeStatus.UpgradeRequired)
            {
                try
                {
                    tenant.SetUpgradeStatus(Controller, eUpgradeStatus.Upgrading);
                    if(await OnDeployTenantSchema(GetTenantController(tenant)))
                    {
                        tenant.SetCheckDate(Controller, DateTime.UtcNow,eUpgradeStatus.Ok);
                    }
                }
                finally
                {
                    tenant.SetUpgradeStatus(Controller, eUpgradeStatus.Ok);
                }
                
            }
            return false;
        }
        public string GetTenantRedirect(int entityType, long entityID,string path)
        {
            Tenant tenant = GetTenant(entityType, entityID);
            return GetTenantRedirect(tenant.Name, path);
        }
        public string GetTenantRedirect(string tenantName,string path)
        {
            return $"http://{(Domains[0].External?"www.":"")}{tenantName}.{Domains[0].Address}/{path}";
        }
        public bool DoesTenantExist(string name)
        {
            return Tenant.Exists(Controller, name);
        }
        public bool DoesTenantExist(int entityType, long entityID)
        {
            return Tenant.Exists(Controller, entityType, entityID);
        }
        public bool IsValidTenantName(string name)
        {
            return Tenant.IsValidTenantName(name);
        }
        public Tenant GetTenant(int entityType, long entityID)
        {
            return Tenant.ForEntity(Controller, entityType, entityID);
        }

        public DateTime GetSchemaCreated()
        {
            System.IO.FileInfo info = new System.IO.FileInfo(SchemaFile);
            if (info != null)
            {
                return info.CreationTimeUtc;
            }
            else
            {
                throw new Exception("file info cannot be null");
            }
        }

        public void CreateTenantLink(long tenantID, byte type, string link)
        {
            Tenant.CreateLink(Controller, tenantID, type, link);
        }

        public void DeleteTenantLink(long tenantID,byte type)
        {
            Tenant.RemoveLink(Controller, tenantID, type);
        }
        
        public Tenant GetTenantFromLink(string link)
        {
            return Tenant.ForLink(Controller, link);
        }

        public string GetLink(long tenantID,byte type)
        {
            return Tenant.GetLink(Controller, tenantID, type);
        }
    }
}
