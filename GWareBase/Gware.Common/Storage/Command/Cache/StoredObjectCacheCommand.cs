using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Cache
{
    public abstract class StoredObjectCacheCommandBase : CacheCommandBase<IStoredObjectCommandController>, IStoredObjectCommandController
    {
        public StoredObjectCacheCommandBase(IStoredObjectCommandController controller)
            :base(controller)
        {

        }

        protected abstract void StoreMultiple<T>(IDataCommand commad, IList<T> items) where T : StoredObjectBase, new();
        protected abstract IList<T> CheckForMultiple<T>(IDataCommand command) where T : StoredObjectBase, new();


        public IList<T> ExecuteMultipleCommand<T>(IDataCommand command) where T : StoredObjectBase, new()
        {
            return ExecuteCacheCommand<IList<T>>(command, CheckForMultiple<T>, Controller.ExecuteMultipleCommand<T>, StoreMultiple);
        }
        
        protected abstract void StoreSingle<T>(IDataCommand command, T item) where T : StoredObjectBase, new();
        protected abstract T CheckForSingle<T>(IDataCommand command) where T : StoredObjectBase, new();

        public T ExecuteSingleCommand<T>(IDataCommand command) where T : StoredObjectBase, new()
        {
            return ExecuteCacheCommand<T>(command, CheckForSingle<T>, Controller.ExecuteSingleCommand<T>, StoreSingle);
        }
    }
}
