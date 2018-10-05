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

            public TenantRequiredAttributeImpl(ITenantWebConfiguration configuration)
            {
                m_configuration = configuration;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                RouteTenant routeTenant = context.HttpContext.Features.Get<RouteTenant>();
                if (routeTenant != null)
                {
                    Tenant tenant = context.HttpContext.Features.Get<Tenant>();
                    if (tenant == null)
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
