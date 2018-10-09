﻿
using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class TenantResolverFilter : TypeFilterAttribute
    {
        public TenantResolverFilter() : base(typeof(TenantResolverFilterImpl))
        {

        }

        private class TenantResolverFilterImpl : IActionFilter
        {

            private readonly ITenantConfiguration m_configuration;
            private readonly ITenantStorage m_storage;

            public TenantResolverFilterImpl(ITenantConfiguration configuration,ITenantStorage storage)
            {
                m_configuration = configuration;
                m_storage = storage;
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
                    m_storage.RouteTenant = tenant;
                    if (tenant != null)
                    {
                        ICommandController controller = m_configuration.Controller;
                        if (controller != null)
                        {
                            m_storage.Tenant = Tenant.ForName(controller, tenant.Name);
                        }
                    }
                }
            }
        }
    }
}
