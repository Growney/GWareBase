using Gware.Standard.Web.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class TenantRequiredIfProvidedAttribute : TypeFilterAttribute
    {
        public TenantRequiredIfProvidedAttribute() : base(typeof(TenantRequiredIfProvidedAttributeImpl))
        {

        }
        private class TenantRequiredIfProvidedAttributeImpl : IActionFilter
        {
            private readonly ITenantWebConfiguration m_configuration;
            private readonly ITenantStorage m_storage;

            public TenantRequiredIfProvidedAttributeImpl(ITenantWebConfiguration configuration,ITenantStorage storage)
            {
                m_configuration = configuration;
                m_storage = storage;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (m_storage?.RouteTenant != null)
                {
                    if (m_storage?.Tenant == null)
                    {
                        context.Result = m_configuration.CreateNewResult;
                    }
                }
            }
        }
    }
}
