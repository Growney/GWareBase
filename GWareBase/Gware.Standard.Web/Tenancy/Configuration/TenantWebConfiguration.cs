using Gware.Standard.Storage.Controller;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public class TenantWebConfiguration : TenantConfiguration, ITenantWebConfiguration
    {
        public IActionResult Upgrading { get; set; }
        public IActionResult NotFoundResult { get; set; }
        public IActionResult TenantHome { get; set; }
        public IActionResult CreateNewResult { get; set; }


        public Task<bool> CreateTenant(string name, string displayName, int entityType, long entityID)
        {
            if (Tenant.IsValidTenantName(name))
            {
                if (!Tenant.Exists(Controller, name))
                {
                    ICommandController newTenancyController = Controller.Clone();
                    newTenancyController.SetName(string.Format(DBNameFormat, name));
                    Tenant.Create(Controller, newTenancyController, name, displayName, entityType, entityID, GetSchemaCreated());

                    return OnDeployTenantSchema(newTenancyController);
                }
            }
            return Task.FromResult(false);
        }


        public bool IsValidTenantName(string name)
        {
            return Tenant.IsValidTenantName(name);
        }
    }
}
