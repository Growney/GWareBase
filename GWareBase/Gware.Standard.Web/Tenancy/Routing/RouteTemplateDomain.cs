using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class RouteTemplateDomain
    {
        public string Address { get; set; }
        public bool IgnorePorts { get; set; }
        public bool External { get; set; }
        public bool Https { get; set; }

        public string GetAddress()
        {
            return $"{(Https?"https":"http")}://{(External ? "www." : "")}{Address}";
        }
    }
}
