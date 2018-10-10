using Gware.Standard.Storage;
using Gware.Standard.Storage.Adapter;
using Gware.Standard.Storage.Command;
using Gware.Standard.Storage.Controller;
using Gware.Standard.Web.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web
{
    public class LinkedTenant : Tenant
    {
        public string Link { get; set; }
        public byte TypeID { get; set; }

        protected override void OnLoad(IDataAdapter adapter)
        {
            base.OnLoad(adapter);
            Link = adapter.GetValue("Link", string.Empty);
            TypeID = adapter.GetValue("TypeID", (byte)0);
        }
        internal static void CreateLink(ICommandController controller, long tenantID, byte type, string link)
        {
            DataCommand command = new DataCommand("TenantLink", "Link");

            command.AddParameter("TenantID", System.Data.DbType.Int64).Value = tenantID;
            command.AddParameter("TypeID", System.Data.DbType.Byte).Value = type;
            command.AddParameter("Link", System.Data.DbType.String).Value = link;

            controller.ExecuteQuery(command);
        }

        internal static void RemoveLink(ICommandController controller, long tenantID, byte type)
        {
            DataCommand command = new DataCommand("TenantLink", "Remove");

            command.AddParameter("TenantID", System.Data.DbType.Int64).Value = tenantID;
            command.AddParameter("TypeID", System.Data.DbType.Byte).Value = type;

            controller.ExecuteQuery(command);
        }

        internal static LinkedTenant ForLink(ICommandController controller, string link)
        {
            DataCommand command = new DataCommand("TenantLink", "SelectTenant");

            command.AddParameter("Link", System.Data.DbType.String).Value = link;

            return LoadSingle<LinkedTenant>(controller.ExecuteCollectionCommand(command));
        }
        internal static List<LinkedTenant> All(ICommandController controller, byte type)
        {
            DataCommand command = new DataCommand("TenantLink", "All");

            command.AddParameter("TypeID", System.Data.DbType.Byte).Value = type;

            return Load<LinkedTenant>(controller.ExecuteCollectionCommand(command));
        }

        internal static string GetLink(ICommandController controller, long tenantID, byte type)
        {
            DataCommand command = new DataCommand("TenantLink", "GetLink");

            command.AddParameter("TenantID", System.Data.DbType.Int64).Value = tenantID;
            command.AddParameter("TypeID", System.Data.DbType.Byte).Value = type;

            return LoadSingle<LoadedFromAdapterValue<string>>(controller.ExecuteCollectionCommand(command))?.Value;
        }
    }
}
