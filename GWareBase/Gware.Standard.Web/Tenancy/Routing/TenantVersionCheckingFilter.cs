using Gware.Standard.Web.Tenancy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class TenantVersionCheckingFilter : TypeFilterAttribute
    {
        
        public TenantVersionCheckingFilter() : base(typeof(TenantVersionCheckingFilterImpl))
        {

        }

        private class TenantVersionCheckingFilterImpl : IActionFilter
        {
            private static object upgradeLock = new object();
            private readonly ITenantWebConfiguration m_configuration;
            private readonly ITenantStorage m_storage;

            public TenantVersionCheckingFilterImpl(ITenantWebConfiguration configuration,ITenantStorage storage)
            {
                m_configuration = configuration;
                m_storage = storage;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                try
                {
                    bool ignore = false;
                    ControllerActionDescriptor controllerDescription = context.ActionDescriptor as ControllerActionDescriptor;
                    if(controllerDescription != null)
                    {
                        ignore = controllerDescription.MethodInfo.GetCustomAttributes(typeof(IgnoreVersionCheckAttribute),false).Length > 0;
                    }
                    if (!ignore)
                    {
                        Tenant currentTenant = m_storage?.Tenant;
                        if (currentTenant != null)
                        {
                            DateTime created = m_configuration.GetSchemaCreated();
                            eUpgradeStatus status = currentTenant.CheckUpgradeStatus(m_configuration.Controller, created);
                            if (status != eUpgradeStatus.Ok)
                            {
                                lock (upgradeLock)
                                {
                                    status = currentTenant.CheckUpgradeStatus(m_configuration.Controller, created);
                                    if(status != eUpgradeStatus.Ok)
                                    {
                                        if (status == eUpgradeStatus.UpgradeRequired)
                                        {
                                            m_configuration.UpgradeTenant(currentTenant, created);
                                        }
                                    }
                                }
                                context.Result = m_configuration.Upgrading(context);
                            }
                        }
                    }
                }
                catch
                {

                }
                
            }
        }
    }
}
