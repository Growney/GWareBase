using Gware.Standard.Storage.Adapter;
using Gware.Standard.Storage.Command;
using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Configuration
{
    public abstract class ConfigurationSetting<T> : Storage.StoredObjectBase where T : Enum
    {
        public override long Id { get; set; }
        public T SettingID
        {
            get
            {
                return (T)(object)Id;
            }
            set
            {
                Id = (long)(object)value;
            }
        }
        public byte[] Value { get; set; }

        protected override void OnLoad(IDataAdapter adapter)
        {
            Value = adapter.GetValue("Value", new byte[0]);
        }

        protected override void AddParametersToSave(IDataCommand command)
        {
            command.AddParameter("Value", System.Data.DbType.Binary).Value = Value;
        }

        public static K Get<K,E>(ICommandController controller, E primaryKey) where K : ConfigurationSetting<E>, new() where E : Enum
        {
            return LoadSingle<K>(controller.ExecuteCollectionCommand(new K().CreateLoadFromPrimaryKey((long)(object)primaryKey)));
        }
    }
}
