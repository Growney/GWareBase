using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Tenancy.Configuration
{
    public class TenantConfiguration : ITenantConfiguration
    {
        public IActionResult NotFoundResult { get; set; }
        public IActionResult NotExistsResult { get; set; }
        public ICommandController Controller { get; set; }
        public string SchemaFile { get; set; }
        public string DBNameFormat { get; set; }

        public async Task<bool> CreateTenant(string name)
        {
            if (!Tenant.Exists(Controller, name))
            {
                ICommandController newTenancyController = Controller.Clone();
                newTenancyController.SetName(string.Format(DBNameFormat, name));
                Tenant.Create(Controller, name, newTenancyController);

                MSSQLCommandController controller = Controller as MSSQLCommandController;
                if (controller != null)
                {
                    string newName = string.Format(DBNameFormat, name);
                    MSSQLCommandController newController = controller.Clone() as MSSQLCommandController;
                    newController.SetName(newName);
                    return await newController.DeploySchema(SchemaFile, newName);
                    
                }
            }
            return false;
        }
    }
}
