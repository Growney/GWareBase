using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.OAuth;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.Tenancy.Configuration
{

    public static class TenantExtensionMethods
    {
        public static T GetLocalArguments<T>(this Microsoft.AspNetCore.Http.HttpContext context, string redirectPath,string returnUrl = "")
            where T : OAuthRequestArguments, new()
        {
            return new T()
            {
                Host = context.Request.Host.ToString(),
                RedirectPath = redirectPath,
                Tenant = context.Features.Get<Tenant>()?.Name,
                ReturnUrl = returnUrl
            };
        }
        public static IServiceCollection AddDelegatedControllerProvider(this IServiceCollection services,Func<string,ICommandController> func)
        {
            return services.AddSingleton<IControllerProvider>(new DelegatedControllerProvider(func));
        }
        public static IMvcBuilder AddTenantMVC(this IServiceCollection services,Action<ITenantConfiguration> tenantConfigurationBuilder, Action<ITenantWebConfiguration> tenantWebConfigBuilder, Action<MvcOptions> mvcConfigBuild)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            TenantWebConfiguration config = new TenantWebConfiguration();

            tenantConfigurationBuilder(config);
            tenantWebConfigBuilder(config);

            services.AddSingleton<ITenantConfiguration>(config);
            services.AddSingleton<ITenantWebConfiguration>(config);

            services.AddScoped<ITenantControllerProvider, TenantControllerProvider>();

            return services.AddMvc(x =>
            {
                x.Filters.Add(new TenantResolverFilter());
                x.Filters.Add(new TenantVersionCheckingFilter());
                mvcConfigBuild(x);
            });
        }
        public static void AddTenant(this IServiceCollection services, Action<ITenantConfiguration> tenantConfigurationBuilder)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            TenantWebConfiguration config = new TenantWebConfiguration();

            tenantConfigurationBuilder(config);

            services.AddSingleton<ITenantConfiguration>(config);

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
