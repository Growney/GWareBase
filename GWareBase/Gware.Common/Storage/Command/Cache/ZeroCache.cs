using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;

namespace Gware.Common.Storage.Command.Cache
{
    public class ZeroCache : CacheCommandBase
    {
        public ZeroCache(ICommandController controller)
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
    }
}
