using Gware.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gware.Tenancy.Routing
{
    public class TenantNotExistsAttribute : TypeFilterAttribute
    {
        public TenantNotExistsAttribute() : base(typeof(TenantNotExistsAttributeImpl))
        {

        }
        private class TenantNotExistsAttributeImpl : IActionFilter
        {
            private readonly ITenantConfiguration m_configuration;

            public TenantNotExistsAttributeImpl(ITenantConfiguration configuration)
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
                    if (tenant != null)
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
