using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Web.Tenancy.Configuration
{
    public class TenantConfiguration<T> : ITenantConfiguration where T : ICommandController
    {

        public ICommandController Controller { get; set; }
        public string SchemaFile { get; set; }
        public string DBNameFormat { get; set; }
        public RouteTemplateDomain[] Domains { get; set; }
        public bool CreateComposite { get; set; }
        public Func<ICommandController, Task<bool>> OnDeployTenantSchema { get; set; }

        private readonly IServiceProvider m_serviceProvider;
        private readonly IControllerProvider m_provider;
        private readonly IConfiguration m_configuration;
        private readonly ILogger<TenantConfiguration<T>> m_logger;

        public TenantConfiguration(IServiceProvider serviceProvider,IControllerProvider provider, IConfiguration configuration, ILogger<TenantConfiguration<T>> logger)
        {
            m_serviceProvider = serviceProvider;
            m_provider = provider;
            m_configuration = configuration;
            m_logger = logger;

            Controller = m_provider.CreateController("TenantDB");
            SchemaFile = m_configuration["TenantConfig:SchemaFile"];
            DBNameFormat = m_configuration["TenantConfig:DBNameFormat"];
            Domains = m_configuration.GetSection("TenantConfig:Domains").Get<RouteTemplateDomain[]>();
        }

        public ICommandController GetTenantController(Tenant tenant)
        {
            m_logger.LogTrace($"Getting tenant controller for tenant {tenant.Name}");
            return tenant?.GetController<T>(m_serviceProvider);
        }

        public string GetDBName(string tenantName)
        {
            return string.Format(DBNameFormat, tenantName);
        }
        public async Task<bool> UpgradeTenant(Tenant tenant, DateTime check)
        {
            eUpgradeStatus status = tenant.CheckUpgradeStatus(Controller, check);
            if (status == eUpgradeStatus.UpgradeRequired)
            {
                try
                {
                    tenant.SetUpgradeStatus(Controller, eUpgradeStatus.Upgrading);
                    m_logger.LogInformation($"Upgrading tenant {tenant.Name}");
                    if (await OnDeployTenantSchema(GetTenantController(tenant)))
                    {
                        m_logger.LogTrace($"Upgrading Tenant {tenant.Name} success");
                        tenant.SetCheckDate(Controller, DateTime.UtcNow, eUpgradeStatus.Ok);
                    }
                    else
                    {
                        m_logger.LogWarning($"Failed to upgrade tenant {tenant.Name}");
                    }
                }
                finally
                {
                    tenant.SetUpgradeStatus(Controller, eUpgradeStatus.Ok);
                }

            }
            return false;
        }
        public string GetTenantRedirect(int entityType, long entityID, string path)
        {
            Tenant tenant = GetTenant(entityType, entityID);
            return GetTenantRedirect(tenant.Name, path);
        }
        public string GetTenantRedirect(string tenantName, string path)
        {
            return $"http://{(Domains[0].External ? "www." : "")}{tenantName}.{Domains[0].Address}/{path}";
        }
        public bool DoesTenantExist(string name)
        {
            return Tenant.Exists(Controller, name);
        }
        public bool DoesTenantExist(int entityType, long entityID)
        {
            return Tenant.Exists(Controller, entityType, entityID);
        }
        public Tenant GetTenant(int entityType, long entityID)
        {
            return Tenant.ForEntity(Controller, entityType, entityID);
        }
        public Tenant GetTenant(long tenantID)
        {
            return Tenant.Get<Tenant>(Controller, tenantID);
        }
        public DateTime GetSchemaCreated()
        {
            System.IO.FileInfo info = new System.IO.FileInfo(SchemaFile);
            if (info != null)
            {
                return info.CreationTimeUtc;
            }
            else
            {
                throw new Exception("file info cannot be null");
            }
        }

        public void CreateTenantLink(long tenantID, byte type, string link)
        {
            LinkedTenant.CreateLink(Controller, tenantID, type, link);
        }

        public void DeleteTenantLink(long tenantID, byte type)
        {
            LinkedTenant.RemoveLink(Controller, tenantID, type);
        }

        public LinkedTenant GetTenantFromLink(string link)
        {
            return LinkedTenant.ForLink(Controller, link);
        }

        public string GetLink(long tenantID, byte type)
        {
            return LinkedTenant.GetLink(Controller, tenantID, type);
        }

        public List<LinkedTenant> AllWithLink(byte type)
        {
            return LinkedTenant.All(Controller, type);
        }
    }
}
