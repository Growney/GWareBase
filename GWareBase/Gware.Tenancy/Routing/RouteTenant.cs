﻿namespace Gware.Tenancy.Routing
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
