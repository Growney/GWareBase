using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.OAuth;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Configuration
{

    public static class TenantExtensionMethods
    {
        public static T GetLocalArguments<T>(this Microsoft.AspNetCore.Http.HttpContext context,Tenant currentTenant, string redirectPath,string returnUrl = "")
            where T : OAuthRequestArguments, new()
        {
            return new T()
            {
                Host = context.Request.Host.ToString(),
                RedirectPath = redirectPath,
                Tenant = currentTenant?.Name,
                ReturnUrl = returnUrl
            };
        }
        public static IServiceCollection AddDelegatedControllerProvider<T>(this IServiceCollection services, Func<ILogger<T>, string, T> func, string defaultKey = "Default") where T : ICommandController
        {
            return services.AddSingleton<IControllerProvider, DelegatedControllerProvider<T>>(x =>
            {
                return new DelegatedControllerProvider<T>(x.GetService<ILogger<T>>(),func, defaultKey);
            });
        }
        public static IServiceCollection AddTenantWebConfiguration<T>(this IServiceCollection services) where T : class, ITenantWebConfiguration
        {
            return AddTenantWebConfiguration<T>(services, null);
        }
        public static IServiceCollection AddTenantWebConfiguration<T>(this IServiceCollection services, Action<T,ITenantConfiguration> configuration) where T : class, ITenantWebConfiguration
        {
            services.AddSingleton<ITenantWebConfiguration, T>(x=>
            {
                T config = ActivatorUtilities.CreateInstance<T>(x);
                configuration?.Invoke(config,x.GetService<ITenantConfiguration>());
                return config;
            });

            return services;
        }
        public static IServiceCollection AddTenantConfiguration<T>(this IServiceCollection services) where T : class, ITenantConfiguration
        {
            return AddTenantConfiguration<T>(services, null);
        }
        public static IServiceCollection AddTenantConfiguration<T>(this IServiceCollection services, Action<T> configuration) where T : class, ITenantConfiguration
        {
            services.AddSingleton<ITenantConfiguration, T>(x =>
            {
                T config = ActivatorUtilities.CreateInstance<T>(x);
                configuration?.Invoke(config);
                return config;
            });

            return services;
        }
        public static IMvcBuilder AddTenantMVC(this IServiceCollection services, Action<MvcOptions> mvcConfigBuild)
        {
            services.AddScoped<ITenantStorage, TenantStorage>();
            services.AddScoped<ITenantControllerProvider, TenantControllerProvider>();

            return services.AddMvc(x =>
            {
                x.Filters.Add(new TenantResolverFilter());
                x.Filters.Add(new TenantVersionCheckingFilter());
                mvcConfigBuild(x);
            });
        }
        public static void AddTenant(this IServiceCollection services)
        {
            services.AddScoped<ITenantStorage, TenantStorage>();
            services.AddScoped<ITenantControllerProvider, TenantControllerProvider>();
        }
        public static IApplicationBuilder UseTenantMvc(this IApplicationBuilder builder,RouteTemplateDomain[] domains,RouteValueDictionary defaults,string template = "{controller}/{action}/{id?}")
        {
            return builder.UseMvc(routes =>
            {
                routes.MapDomainRoutes(
                    domains: domains,
                    routeTemplate: template,
                    defaults: defaults
                    );

                routes.MapRoute(
                    name: "default",
                    defaults: defaults,
                    template: template);
            });
        }
    }
    
}
