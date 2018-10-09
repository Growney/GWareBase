using Gware.Standard.Web.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class TenantNotExistsAttribute : TypeFilterAttribute
    {
        public TenantNotExistsAttribute() : base(typeof(TenantNotExistsAttributeImpl))
        {

        }
        private class TenantNotExistsAttributeImpl : IActionFilter
        {
            private readonly ITenantWebConfiguration m_configuration;
            private readonly ITenantStorage m_storage;

            public TenantNotExistsAttributeImpl(ITenantWebConfiguration configuration,ITenantStorage storage)
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
                    if (m_storage?.Tenant != null)
                    {
                        context.Result = m_configuration.TenantHome;
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
