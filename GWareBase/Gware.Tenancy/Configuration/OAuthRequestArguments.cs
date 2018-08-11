using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Configuration
{
    public abstract class OAuthRequestArguments
    {
        public string RedirectPath { get; set; }
        public string Host { get; set; }
        public string Tenant { get; set; }
        public string Code { get; set; }

        public abstract string GetAuthenticationUrl(IOAuthConfiguration config, string state);
        public string GetRedirectString(string state)
        {
            return $"http://{Host}/{RedirectPath}?state={state}";
        }
    }

    public static class AuthRequestArgumentsExtensionMethods
    {
        public static T GetLocalArguments<T>(this Microsoft.AspNetCore.Http.HttpContext context, string redirectPath) 
            where T : OAuthRequestArguments,new()
        {
            return new T()
            {
                Host = context.Request.Host.ToString(),
                RedirectPath = redirectPath,
                Tenant = context.Features.Get<Tenant>()?.Name
            };
        }
    }
}
