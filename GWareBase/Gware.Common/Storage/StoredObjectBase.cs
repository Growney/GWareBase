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
    public abstract class StoredObjectBase : CommandStoredBase, ISaveable,IHasID
    {
        public abstract long Id { get; set; }
        

        public StoredObjectBase(bool disableDirtyCheck = true)
            :base(disableDirtyCheck)
        {

        }

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

        protected virtual void OnSave(ICommandController controller)
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

        public virtual long Save(ICommandController controller)
        {
            long retVal = Id;
            if (DisabledDirtyCheck ||  GetIsDirty())
            {
                IDataCommand command = CreateSaveCommand();
                command.AddReCacheCommand(GetSaveReCacheCommands());
                command.Cache = false;
                retVal = LoadSingle<LoadedFromAdapterValue<long>>(controller.ExecuteCollectionCommand(command)).Value;
                Id = retVal;
            }

            OnSave(controller);
            return retVal;
        }

        public virtual bool Delete(ICommandController controller)
        {
            IDataCommand command = CreateDeleteCommand();
            command.AddReCacheCommand(GetDeleteReCacheCommands());
            return LoadSingle<LoadedFromAdapterValue<bool>>(controller.ExecuteCollectionCommand(command)).Value;
        }


        public virtual void Load(ICommandController controller,long primaryKey)
        {
            IDataCommand command = CreateLoadFromPrimaryKey(primaryKey);
            LoadFrom(controller.ExecuteCollectionCommand(command).First);
        }

        
        public static T Get<T>(ICommandController controller,long primaryKey) where T : StoredObjectBase,new()
        {
            T retVal = new T();
            retVal.Load(controller,primaryKey);
            return retVal;
        }
    }

    public class StoredObjectIDComparer : IEqualityComparer<StoredObjectBase>
    {
        public bool Equals(StoredObjectBase x, StoredObjectBase y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(StoredObjectBase obj)
        {
            return obj.GetHashCode();
        }
    }
}
