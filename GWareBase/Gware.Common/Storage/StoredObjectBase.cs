using Gware.Common.Application;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public abstract class StoredObjectBase : CommandStoredBase
    {
        private int m_id;

        public int Id
        {
            get
            {
                return m_id;
            }

            set
            {
                m_id = value;
            }
        }

        protected override void LoadFrom(IDataAdapter adapter)
        {
            m_id = adapter.GetValue("Id", 0);
            OnLoad(adapter);
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

        public virtual int Save()
        {
            int retVal = Id;
            if (Dirty)
            {
                IDataCommand command = CreateSaveCommand();
                command.AddReCacheCommand(GetSaveReCacheCommands());
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
        
    }
}
