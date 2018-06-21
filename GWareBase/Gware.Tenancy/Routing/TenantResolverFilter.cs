using Gware.Common.Storage.Command.Interface;
using Gware.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Routing
{
    public class TenantResolverFilter : TypeFilterAttribute
    {
        public TenantResolverFilter() : base(typeof(TenantResolverFilterImpl))
        {

        }

        private class TenantResolverFilterImpl : IActionFilter
        {

            private readonly ITenantConfiguration m_configuration;

            public TenantResolverFilterImpl(ITenantConfiguration configuration)
            {
                m_configuration = configuration;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                RouteData data = context.HttpContext.GetRouteData();
                if (data.Values.ContainsKey("tenant"))
                {
                    RouteTenant tenant = new RouteTenant(data.Values["tenant"].ToString());
                    context.HttpContext.Features.Set(tenant);
                    if (tenant != null)
                    {
                        ICommandController controller = m_configuration.Controller;
                        if (controller != null)
                        {
                            context.HttpContext.Features.Set<Tenant>(Tenant.ForName(controller, tenant.Name));
                        }
                    }
                }
            }
        }
    }
}
