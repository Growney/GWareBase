using Gware.Standard.Storage.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public class TenantWebConfiguration : TenantConfiguration, ITenantWebConfiguration
    {
        public Func<ActionExecutingContext, IActionResult> Upgrading { get; set; }
        public IActionResult NotFoundResult { get; set; }
        public IActionResult TenantHome { get; set; }
        public IActionResult CreateNewResult { get; set; }


        public async Task<bool> CreateTenant(string name, string displayName, int entityType, long entityID)
        {
            bool retVal = false;
            if (Tenant.IsValidTenantName(name))
            {
                if (!Tenant.Exists(Controller, name))
                {
                    ICommandController newTenancyController = Controller.Clone();
                    newTenancyController.SetName(string.Format(DBNameFormat, name));
                    long? id = Tenant.Create(Controller, newTenancyController, name, displayName, entityType, entityID, GetSchemaCreated())?.Id;

                    if(id.HasValue)
                    {
                        Task<bool> deploy = OnDeployTenantSchema(newTenancyController);

                        if (!await deploy)
                        {
                            Tenant.Delete(Controller, id.Value);
                        }
                        else
                        {
                            retVal = true;
                        }
                    } 
                }
            }
            return retVal;
        }


        public bool IsValidTenantName(string name)
        {
            return Tenant.IsValidTenantName(name);
        }
    }
}
