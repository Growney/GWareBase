using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.OAuth
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
}
