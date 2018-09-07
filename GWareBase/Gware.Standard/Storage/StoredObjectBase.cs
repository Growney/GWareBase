using Gware.Standard.Storage.Command;
using Gware.Standard.Storage.Controller;
using Gware.Standard.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Storage
{
    public abstract class StoredObjectBase : LoadedFromAdapterBase, ISaveable, IHasID
    {
        public abstract long Id { get; set; }

        protected virtual string GetIDField()
        {
            return "Id";
        }

        
        public virtual long Save(ICommandController controller)
        { 
            Id = LoadSingle<LoadedFromAdapterValue<long>>(controller.ExecuteCollectionCommand(CreateSaveCommand())).Value; 
            OnSave(controller);
            return Id;
        }

        protected abstract void OnLoad(IDataAdapter adapter);
        protected virtual void AddParametersToSave(IDataCommand command)
        {

        }
        protected override void LoadFrom(IDataAdapter adapter)
        {
            if (adapter != null)
            {
                Id = adapter.GetValue(GetIDField(), 0);
                OnLoad(adapter);
            }
        }
        protected virtual void OnSave(ICommandController controller)
        {

        }

        public virtual IDataCommand CreateSaveCommand()
        {
            DataCommand command = new DataCommand(GetType().Name, "Save");
            command.AddParameter(GetIDField(), System.Data.DbType.Int64).Value = Id;
            AddParametersToSave(command);
            return command;
        }
        public virtual void Delete(ICommandController controller)
        {
            IDataCommand command = CreateDeleteCommand();
            controller.ExecuteQuery(command);
            OnDelete(controller);
        }
        public Task DeleteAsync(ICommandController controller)
        {
            return Task.Factory.StartNew(() =>
            {
                Delete(controller);
            });
        }

        public virtual void OnDelete(ICommandController controller)
        {

        }
        public virtual IDataCommand CreateDeleteCommand()
        {
            DataCommand command = new DataCommand(GetType().Name, "Delete");
            command.AddParameter(GetIDField(), System.Data.DbType.Int64).Value = Id;
            return command;
        }

        public virtual void Load(ICommandController controller, long primaryKey)
        {
            IDataCommand command = CreateLoadFromPrimaryKey(primaryKey);
            LoadFrom(controller.ExecuteCollectionCommand(command).First);
        }

        public virtual IDataCommand CreateLoadFromPrimaryKey(long primaryKey)
        {
            DataCommand command = new DataCommand(GetType().Name, "Single");
            command.AddParameter(GetIDField(), System.Data.DbType.Int64).Value = primaryKey;
            return command;
        }

        public static T Get<T>(ICommandController controller, long primaryKey) where T : StoredObjectBase, new()
        {
            T retVal = new T();
            retVal.Load(controller, primaryKey);
            return retVal;
        }

        public Task<long> SaveAsync(ICommandController controller)
        {
            throw new NotImplementedException();
        }
    }
}
