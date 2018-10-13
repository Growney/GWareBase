using Gware.Standard.Storage.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public class TenantWebConfiguration :  ITenantWebConfiguration
    {
        public Func<ActionExecutingContext, IActionResult> Upgrading { get; set; }
        public IActionResult NotFoundResult { get; set; }
        public IActionResult TenantHome { get; set; }
        public IActionResult CreateNewResult { get; set; }

        private readonly ITenantConfiguration m_tenantConfiguration;

        public TenantWebConfiguration(ITenantConfiguration tenantConfig)
        {
            m_tenantConfiguration = tenantConfig;
        }

        public async Task<bool> CreateTenant(string name, string displayName, int entityType, long entityID)
        {
            bool retVal = false;
            if (Tenant.IsValidTenantName(name))
            {
                if (!Tenant.Exists(m_tenantConfiguration.Controller, name))
                {
                    ICommandController newTenancyController = m_tenantConfiguration.Controller.Clone();
                    newTenancyController.SetName(string.Format(m_tenantConfiguration.DBNameFormat, name));
                    long? id = Tenant.Create(m_tenantConfiguration.Controller, newTenancyController, name, displayName, entityType, entityID, m_tenantConfiguration.GetSchemaCreated())?.Id;

                    if(id.HasValue)
                    {
                        Task<bool> deploy = m_tenantConfiguration.OnDeployTenantSchema(newTenancyController);

                        if (!await deploy)
                        {
                            Tenant.Delete(m_tenantConfiguration.Controller, id.Value);
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
