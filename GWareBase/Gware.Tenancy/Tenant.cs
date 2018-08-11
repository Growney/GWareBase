using System;
using System.Collections.Generic;
using System.Text;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Tenancy
{
    public class Tenant : Gware.Common.Storage.StoredObjectBase
    {
        public override long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string ControllerCreationString { get; set; }
        public string DisplayName { get; set; }
        public int EntityType { get; set; }
        public long EntityId { get; set; }
        public DateTime UpgradeCheck { get; private set; }
        public eUpgradeStatus UpgradeStatus { get; private set; }
        public ICommandController Controller
        {
            get
            {
                return CommandControllerFactory.CreateController(ControllerCreationString);
            }

        }

        public override IDataCommand CreateDeleteCommand()
        {
            DataCommand command = new DataCommand("Tenant", "Delete");
            command.AddParameter("Id", System.Data.DbType.Int64).Value = Id;
            return command;
        }

        public override IDataCommand CreateLoadFromPrimaryKey(long primaryKey)
        {
            DataCommand command = new DataCommand("Tenant", "Single");
            command.AddParameter("Id", System.Data.DbType.Int64).Value = primaryKey;
            return command;
        }

        public override IDataCommand CreateSaveCommand()
        {
            DataCommand command = new DataCommand("Tenant", "Save");
            command.AddParameter("Name", System.Data.DbType.String).Value = Name;
            command.AddParameter("ControllerCreationString", System.Data.DbType.String).Value = ControllerCreationString;
            command.AddParameter("DisplayName", System.Data.DbType.String).Value = DisplayName;
            command.AddParameter("EntityType", System.Data.DbType.Int32).Value = EntityType;
            command.AddParameter("EntityID", System.Data.DbType.Int64).Value = EntityId;
            command.AddParameter("UpgradeCheck", System.Data.DbType.DateTime).Value = UpgradeCheck;
            command.AddParameter("UpgradeStatus", System.Data.DbType.Int16).Value = (byte)UpgradeStatus;
            return command;
        }

        protected override void OnLoad(IDataAdapter adapter)
        {
            Name = adapter.GetValue("Name", string.Empty);
            Created = adapter.GetValue("Created", DateTime.MinValue);
            ControllerCreationString = adapter.GetValue("ControllerCreationString", string.Empty);
            DisplayName = adapter.GetValue("DisplayName", string.Empty);
            EntityType = adapter.GetValue("EntityType", 0);
            EntityId = adapter.GetValue("EntityID", 0L);
            UpgradeCheck = adapter.GetValue("UpdateCheck", DateTime.MinValue);
            UpgradeStatus = (eUpgradeStatus)adapter.GetValue("UpgradeStatus", 0);
        }

        internal eUpgradeStatus CheckUpgradeStatus(ICommandController controller,DateTime checkAgainst)
        {
            DataCommand command = new DataCommand("Tenant", "CheckUpgradeStatus");
            command.AddParameter("Id", System.Data.DbType.Int64).Value = Id;
            command.AddParameter("CheckAgainst", System.Data.DbType.DateTime).Value = checkAgainst;
            UpgradeStatus = (eUpgradeStatus)LoadSingle<LoadedFromAdapterValue<int>>(controller.ExecuteCollectionCommand(command)).Value;
            return UpgradeStatus;
        }

        internal void SetUpgradeStatus(ICommandController controller, eUpgradeStatus status)
        {
            DataCommand command = new DataCommand("Tenant", "SetUpgradeStatus");
            command.AddParameter("Id", System.Data.DbType.Int64).Value = Id;
            command.AddParameter("UpgradeStatus", System.Data.DbType.Int16).Value = (byte)status;
            UpgradeStatus = (eUpgradeStatus)LoadSingle<LoadedFromAdapterValue<int>>(controller.ExecuteCollectionCommand(command)).Value;
        }

        internal void SetCheckDate(ICommandController controller,DateTime date, eUpgradeStatus status)
        {
            DataCommand command = new DataCommand("Tenant", "SetCheckDate");
            command.AddParameter("Id", System.Data.DbType.Int64).Value = Id;
            command.AddParameter("UpgradeCheck", System.Data.DbType.DateTime).Value = date;
            command.AddParameter("UpgradeStatus", System.Data.DbType.Int16).Value = (byte)status;
            controller.ExecuteQuery(command);
        }

        internal static bool Exists(ICommandController controller,string name)
        {
            DataCommand command = new DataCommand("Tenant", "Exists");
            command.AddParameter("Name", System.Data.DbType.String).Value = name;
            return LoadSingle<Common.Storage.LoadedFromAdapterValue<int>>(controller.ExecuteCollectionCommand(command)).Value == 1;
        }
        internal static bool Exists(ICommandController controller, int entityType, long entityID)
        {
            DataCommand command = new DataCommand("Tenant", "EntityExists");
            command.AddParameter("EntityType", System.Data.DbType.Int32).Value = entityType;
            command.AddParameter("EntityID", System.Data.DbType.Int64).Value = entityID;
            return LoadSingle<Common.Storage.LoadedFromAdapterValue<int>>(controller.ExecuteCollectionCommand(command)).Value == 1;
        }
        internal static Tenant ForName(ICommandController controller,string name)
        {
            DataCommand command = new DataCommand("Tenant", "ForName");
            command.AddParameter("Name", System.Data.DbType.String).Value = name;
            return LoadSingle<Tenant>(controller.ExecuteCollectionCommand(command));
        }
        internal static Tenant ForEntity(ICommandController controller, int entityType, long entityID)
        {
            DataCommand command = new DataCommand("Tenant", "ForEntity");
            command.AddParameter("EntityType", System.Data.DbType.Int32).Value = entityType;
            command.AddParameter("EntityID", System.Data.DbType.Int64).Value = entityID;
            return LoadSingle<Tenant>(controller.ExecuteCollectionCommand(command));
        }
        internal static Tenant Create(ICommandController controller, ICommandController useController,string name, string displayname,int entityType, long entityID,DateTime upgradeCheck)
        {
            Tenant retVal = new Tenant()
            {
                Name = name,
                Created = DateTime.UtcNow,
                ControllerCreationString = CommandControllerFactory.GetCreationString(useController),
                DisplayName = displayname,
                EntityId = entityID,
                EntityType = entityType,
                UpgradeCheck = upgradeCheck
            };

            retVal.Save(controller);

            return retVal;
        }
        
        internal static void CreateLink(ICommandController controller,long tenantID,byte type,string link)
        {
            DataCommand command = new DataCommand("TenantLink", "Link");

            command.AddParameter("TenantID", System.Data.DbType.Int64).Value = tenantID;
            command.AddParameter("TypeID", System.Data.DbType.Byte).Value = type;
            command.AddParameter("Link", System.Data.DbType.String).Value = link;

            controller.ExecuteQuery(command);
        }

        internal static void RemoveLink(ICommandController controller,long tenantID,byte type)
        {
            DataCommand command = new DataCommand("TenantLink", "Remove");

            command.AddParameter("TenantID", System.Data.DbType.Int64).Value = tenantID;
            command.AddParameter("TypeID", System.Data.DbType.Byte).Value = type;

            controller.ExecuteQuery(command);
        }

        internal static Tenant ForLink(ICommandController controller, string link)
        {
            DataCommand command = new DataCommand("TenantLink", "SelectTenant");

            command.AddParameter("Link", System.Data.DbType.String).Value = link;

            return LoadSingle<Tenant>(controller.ExecuteCollectionCommand(command));
        }

        public static bool IsValidTenantName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetterOrDigit(name[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static string MakeValidTenantName(string name)
        {
            StringBuilder retVal = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (Char.IsLetterOrDigit(name[i]))
                {
                    retVal.Append(name[i].ToString().ToLower());
                }
            }
            return retVal.ToString();
        }

        
    }
}
