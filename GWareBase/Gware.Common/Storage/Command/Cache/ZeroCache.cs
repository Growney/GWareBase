using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Common.Storage.Command.Cache
{
    public class ZeroCache : StoredObjectCacheCommandBase
    {
        public ZeroCache(IStoredObjectCommandController controller)
            :base(controller)
        {

        }
        protected override IDataAdapterCollection CheckForColllection(IDataCommand command)
        {
            return null;
        }

        protected override IDataAdapterCollectionGroup CheckForGroup(IDataCommand command)
        {
            return null;
        }

        protected override IList<T> CheckForMultiple<T>(IDataCommand command)
        {
            return null;
        }

        protected override T CheckForSingle<T>(IDataCommand command)
        {
            return null;
        }

        protected override void ClearCache()
        {
            
        }

        protected override void Recache(IDataCommand command)
        {
            
        }

        protected override void StoreCollection(IDataCommand command, IDataAdapterCollection group)
        {
            
        }

        protected override void StoreGroup(IDataCommand command, IDataAdapterCollectionGroup group)
        {
            
        }

        protected override void StoreMultiple<T>(IDataCommand commad, IList<T> items)
        {
            throw new NotImplementedException();
        }

        protected override void StoreSingle<T>(IDataCommand command, T item)
        {
            throw new NotImplementedException();
        }
    }
}
