using Gware.Standard.Web.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class RouteTenantRequiredAttribute : TypeFilterAttribute
    {
        public RouteTenantRequiredAttribute() : base(typeof(RouteTenantRequiredAttributeImpl))
        {

        }
        private class RouteTenantRequiredAttributeImpl : IActionFilter
        {
            private readonly ITenantConfiguration m_configuration;

            public RouteTenantRequiredAttributeImpl(ITenantConfiguration configuration)
            {
                m_configuration = configuration;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                RouteTenant routeTenant = context.HttpContext.Features.Get<RouteTenant>();
                if (routeTenant == null)
                {
                    context.Result = m_configuration.NotFoundResult;
                }
            }
        }


    }
}
