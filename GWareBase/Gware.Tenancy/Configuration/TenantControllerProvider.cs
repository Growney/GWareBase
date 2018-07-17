using Gware.Common.Context;
using Gware.Common.Storage.Command.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Configuration
{
    public class TenantControllerProvider : ITenantControllerProvider
    {
        private readonly IHttpContextAccessor m_context;
        private readonly IControllerProvider m_defaultProvider;
        private readonly ITenantConfiguration m_tenantConfiguration;

        public TenantControllerProvider(IHttpContextAccessor context,IControllerProvider defaultProvider,ITenantConfiguration tenantConfigration)
        {
            m_context = context;
            m_defaultProvider = defaultProvider;
            m_tenantConfiguration = tenantConfigration;
        }

        public ICommandController GetController()
        {
            ICommandController retVal = null;
            Tenant currentTenant = m_context.HttpContext.Features.Get<Tenant>();
            if(currentTenant != null)
            {
                retVal = currentTenant.Controller;
            }
            else
            {
                retVal = m_defaultProvider?.CreateController(m_tenantConfiguration.ControllerKey);
            }
            return retVal;
        }
    }
}
