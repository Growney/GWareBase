using System;
using System.Collections.Generic;
using System.Text;
using Gware.Common.Application;
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
        public string ImageSource { get; set; }
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
            return command;
        }

        protected override void OnLoad(IDataAdapter adapter)
        {
            Name = adapter.GetValue("Name", string.Empty);
            Created = adapter.GetValue("Created", DateTime.MinValue);
            ControllerCreationString = adapter.GetValue("ControllerCreationString", string.Empty);
            DisplayName = adapter.GetValue("DisplayName", string.Empty);
        }

        internal static bool Exists(ICommandController controller,string name)
        {
            DataCommand command = new DataCommand("Tenant", "Exists");
            command.AddParameter("Name", System.Data.DbType.String).Value = name;
            return LoadSingle<Common.Storage.LoadedFromAdapterValue<int>>(controller.ExecuteCollectionCommand(command)).Value == 1;
        }
        internal static Tenant ForName(ICommandController controller,string name)
        {
            DataCommand command = new DataCommand("Tenant", "ForName");
            command.AddParameter("Name", System.Data.DbType.String).Value = name;
            return LoadSingle<Tenant>(controller.ExecuteCollectionCommand(command));
        }
        internal static Tenant Create(ICommandController controller, ICommandController useController,string name, string displayname,string imagesource)
        {
            Tenant retVal = new Tenant()
            {
                Name = name,
                Created = DateTime.UtcNow,
                ControllerCreationString = CommandControllerFactory.GetCreationString(useController),
                DisplayName = displayname,
                ImageSource = imagesource
            };

            retVal.Save(controller);

            return retVal;
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
