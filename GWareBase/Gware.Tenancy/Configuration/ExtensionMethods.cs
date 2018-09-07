namespace Gware.Tenancy.Configuration
{

    public static class AuthRequestArgumentsExtensionMethods
    {
        public static T GetLocalArguments<T>(this Microsoft.AspNetCore.Http.HttpContext context, string redirectPath)
            where T : OAuthRequestArguments, new()
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
