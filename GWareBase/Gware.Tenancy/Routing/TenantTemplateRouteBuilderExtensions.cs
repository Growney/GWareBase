using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gware.Tenancy.Routing
{
    public static class TenantTemplateRouteBuilderExtensions
    {
        public static IRouteBuilder MapDomainRoute(this IRouteBuilder routeCollectionBuilder, string name, string domainTemplate, string routeTemplate)
        {
            MapDomainRoute(routeCollectionBuilder, name, domainTemplate, routeTemplate, null);
            return routeCollectionBuilder;
        }

        public static IRouteBuilder MapDomainRoute(this IRouteBuilder routeCollectionBuilder, string name, string domainTemplate, string routeTemplate, RouteValueDictionary defaults, bool ignorePort = true)
        {
            return MapDomainRoute(routeCollectionBuilder, name, domainTemplate, routeTemplate, defaults, null, ignorePort);
        }

        public static IRouteBuilder MapDomainRoute(this IRouteBuilder routeCollectionBuilder, string name, string domainTemplate, string routeTemplate, RouteValueDictionary defaults, object constraints, bool ignorePort = true)
        {
            return MapDomainRoute(routeCollectionBuilder, name, domainTemplate, routeTemplate, defaults, constraints, null, ignorePort);
        }

        public static IRouteBuilder MapDomainRoute(this IRouteBuilder routeCollectionBuilder, string name, string domainTemplate, string routeTemplate, RouteValueDictionary defaults, object constraints, RouteValueDictionary dataTokens, bool ignorePort = true)
        {
            if (routeCollectionBuilder.DefaultHandler == null)
                throw new InvalidOperationException("Default handler must be set.");
            var inlineConstraintResolver = routeCollectionBuilder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>();
            routeCollectionBuilder.Routes.Add(new TenantTemplateRoute(routeCollectionBuilder.DefaultHandler, name, domainTemplate, routeTemplate, defaults, ObjectToDictionary(constraints), dataTokens, ignorePort, inlineConstraintResolver));
            return routeCollectionBuilder;
        }

        public static IRouteBuilder MapDomainRoutes(this IRouteBuilder routeCollectionBuilder, string[] domains, string routeTemplate, RouteValueDictionary defaults,bool ignorePorts = true)
        {
            if (domains != null)
            {
                for (int i = 0; i < domains.Length; i++)
                {
                    string domain = domains[i];

                    routeCollectionBuilder.MapDomainRoute(
                      name: $"{domain}TenantRoute",
                      domainTemplate: "{tenant}." + domain,
                      routeTemplate: routeTemplate,
                      defaults: defaults,
                      ignorePort:ignorePorts
                      );
                }
            }
            return routeCollectionBuilder;
        }

        private static IDictionary<string, object> ObjectToDictionary(object value)
        {
            return value as IDictionary<string, object> ?? new RouteValueDictionary(value);
        }
    }
}
