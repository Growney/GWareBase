using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Routing
{
    public class RouteTenant
    {
        public string Name { get; private set; }
        public RouteTenant(string name)
        {
            Name = name;
        }
    }
}
