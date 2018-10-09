using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public interface ITenantStorage
    {
        Tenant Tenant { get; set; }
        RouteTenant RouteTenant { get; set; }
    }
}
