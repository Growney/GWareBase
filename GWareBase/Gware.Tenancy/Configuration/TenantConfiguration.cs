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
        public IActionResult TenantHome { get; set; }
        public IActionResult CreateNewResult { get; set; }
        public ICommandController Controller { get; set; }
        public string SchemaFile { get; set; }
        public string DBNameFormat { get; set; }
        public string ControllerKey { get; set; }
        public string[] Domains { get; set; }

        public TenantConfiguration()
        {
            ControllerKey = "tenant";
        }

        public async Task<bool> CreateTenant(string name,string displayName,string imagesource, bool createComposite = false)
        {
            if (Tenant.IsValidTenantName(name))
            {
                if (!Tenant.Exists(Controller, name))
                {
                    ICommandController newTenancyController = Controller.Clone();
                    newTenancyController.SetName(string.Format(DBNameFormat, name));
                    Tenant.Create(Controller, newTenancyController, name, displayName,imagesource);

                    MSSQLCommandController controller = Controller as MSSQLCommandController;
                    if (controller != null)
                    {
                        string dbName = string.Format(DBNameFormat, name);
                        MSSQLCommandController newController = controller.Clone() as MSSQLCommandController;
                        newController.SetName(dbName);
                        return await newController.DeploySchema(SchemaFile, dbName, createComposite);

                    }
                }
            }
            return false;
        }

        public string GetTenantRedirect(string tenantName,string path)
        {
            return $"http://{tenantName}.{Domains[0]}/{path}";
        }
        public bool DoesTenantExists(string name)
        {
            return Tenant.Exists(Controller, name);
        }

        public bool IsValidTenantName(string name)
        {
            return Tenant.IsValidTenantName(name);
        }
}
}
