using Gware.Common.Application;
using Gware.Common.Reflection;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public abstract class StoredObjectBase : CommandStoredBase
    {
        public abstract long Id { get; set; }
        

        protected virtual string GetIDField()
        {
            return "Id";
        }

        protected override void LoadFrom(IDataAdapter adapter)
        {
            if(adapter != null)
            {
                Id = adapter.GetValue(GetIDField(), 0);
                OnLoad(adapter);
            }
        }

        protected abstract void OnLoad(IDataAdapter adapter);

        protected virtual void OnSave()
        {

        }

        protected internal virtual IDataCommand[] GetSaveReCacheCommands()
        {
            return new IDataCommand[0];
        }

        protected internal virtual IDataCommand[] GetDeleteReCacheCommands()
        {
            return new IDataCommand[0];
        }

        protected override bool GetIsDirty()
        {
            if(Id == 0)
            {
                return true;
            }
            return base.GetIsDirty();
        }

        public virtual long Save()
        {
            long retVal = Id;
            if (GetIsDirty())
            {
                IDataCommand command = CreateSaveCommand();
                command.AddReCacheCommand(GetSaveReCacheCommands());
                command.Cache = false;
                retVal = LoadSingle<LoadedFromAdapterValue<int>>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(command)).Value;
                Id = retVal;
            }

            OnSave();
            return retVal;
        }

        public virtual bool Delete()
        {
            IDataCommand command = CreateDeleteCommand();
            command.AddReCacheCommand(GetDeleteReCacheCommands());
            return LoadSingle<LoadedFromAdapterValue<bool>>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(command)).Value;
        }


        public virtual void Load(long primaryKey)
        {
            IDataCommand command = CreateLoadFromPrimaryKey(primaryKey);
            LoadFrom(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(command).First);
        }

        public static T Get<T>(long primaryKey) where T : StoredObjectBase,new()
        {
            T retVal = new T();
            retVal.Load(primaryKey);
            return retVal;
        }

    }
}
