using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public class TenantControllerProvider : ITenantControllerProvider
    {
        private readonly ITenantStorage m_storage;
        private readonly IControllerProvider m_defaultProvider;
        private readonly ITenantConfiguration m_tenantConfiguration;

        public TenantControllerProvider(ITenantStorage storage,IControllerProvider defaultProvider,ITenantConfiguration tenantConfigration)
        {
            m_storage = storage;
            m_defaultProvider = defaultProvider;
            m_tenantConfiguration = tenantConfigration;
        }
        
        public ICommandController GetController()
        {
            ICommandController retVal = null;
            Tenant currentTenant = m_storage?.Tenant;
            if (currentTenant != null)
            {
                retVal = m_tenantConfiguration.GetTenantController(currentTenant);
            }
            else
            {
                retVal = m_defaultProvider?.GetDefaultDataController();
            }
            return retVal;
        }
    }
}
