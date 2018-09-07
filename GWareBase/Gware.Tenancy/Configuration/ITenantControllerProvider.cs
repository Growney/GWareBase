using Gware.Common.Storage.Command.Interface;
using Microsoft.AspNetCore.Http;

namespace Gware.Tenancy.Configuration
{
    public interface ITenantControllerProvider
    {
        ICommandController GetController();
        ICommandController GetController(HttpContext context);
    }
}
