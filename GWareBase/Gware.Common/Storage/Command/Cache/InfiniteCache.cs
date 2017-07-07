using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;
using Gware.Common.DataStructures;

namespace Gware.Common.Storage.Command.Cache
{
    public class InfiniteCache : CacheCommandBase
    {
        private HashedDictionary<IDataCommand, IDataAdapterCollectionGroup> m_groupCache;
        private HashedDictionary<IDataCommand, IDataAdapterCollection> m_collectionCache;
        public InfiniteCache(ICommandController controller) : base(controller)
        {
            m_groupCache = new HashedDictionary<IDataCommand, IDataAdapterCollectionGroup>();
            m_collectionCache = new HashedDictionary<IDataCommand, IDataAdapterCollection>();
        }

        protected override IDataAdapterCollection CheckForColllection(IDataCommand command)
        {
            return m_collectionCache.Get(command);

        }

        protected override IDataAdapterCollectionGroup CheckForGroup(IDataCommand command)
        {
            return m_groupCache.Get(command);
        }

        protected override void ClearCache()
        {
            m_groupCache.Clear();
            m_collectionCache.Clear();
        }

        protected override void Recache(IDataCommand command)
        {
            m_groupCache.Remove(command);
            m_collectionCache.Remove(command);
        }

        protected override void StoreCollection(IDataCommand command, IDataAdapterCollection group)
        {
            m_collectionCache.Add(command, group);
        }

        protected override void StoreGroup(IDataCommand command, IDataAdapterCollectionGroup group)
        {
            m_groupCache.Add(command, group);
        }
    }
}
