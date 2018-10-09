using Gware.Standard.Web.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class TenantRequiredAttribute : TypeFilterAttribute
    {
        public TenantRequiredAttribute() : base(typeof(TenantRequiredAttributeImpl))
        {

        }
        private class TenantRequiredAttributeImpl : IActionFilter
        {
            private readonly ITenantWebConfiguration m_configuration;
            private readonly ITenantStorage m_storage;

            public TenantRequiredAttributeImpl(ITenantWebConfiguration configuration,ITenantStorage storage)
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
                else
                {
                    context.Result = m_configuration.NotFoundResult;
                }
            }
        }


    }
}
