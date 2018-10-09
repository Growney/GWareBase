using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class TenantStorage : ITenantStorage
    {
        public Tenant Tenant { get; set; }
        public RouteTenant RouteTenant { get; set; }
    }
}
