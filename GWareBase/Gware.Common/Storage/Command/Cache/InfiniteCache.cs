using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;
using Gware.Common.DataStructures;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Common.Storage.Command.Cache
{
    public class InfiniteCache : StoredObjectCacheCommandBase
    {
        private IHashedDictionary<IDataCommand,IDataAdapterCollectionGroup> m_groupCache;
        private IHashedDictionary<IDataCommand, IDataAdapterCollection> m_collectionCache;

        private IHashedDictionary<IDataCommand, List<StoredObjectBase>> m_multipleCache;
        private IHashedDictionary<IDataCommand, StoredObjectBase> m_singleCache;

        public InfiniteCache(IStoredObjectCommandController controller) : base(controller)
        {
            m_groupCache = new CachedCommandHashedDictonary<IDataAdapterCollectionGroup> ();
            m_collectionCache = new CachedCommandHashedDictonary<IDataAdapterCollection> ();
            m_singleCache = new CachedCommandHashedDictonary<StoredObjectBase>();
            m_multipleCache = new CachedCommandHashedDictonary<List<StoredObjectBase>>();
        }

        protected override IDataAdapterCollection CheckForColllection(IDataCommand command)
        {
            return m_collectionCache.Get(command);

        }

        protected override IDataAdapterCollectionGroup CheckForGroup(IDataCommand command)
        {
            return m_groupCache.Get(command);
        }

        protected override IList<T> CheckForMultiple<T>(IDataCommand command)
        {
            List<StoredObjectBase> multiple = m_multipleCache.Get(command);
            if(multiple != null)
            {
                return multiple.Convert<StoredObjectBase,T>();
            }
            return null;
        }

        protected override T CheckForSingle<T>(IDataCommand command)
        {
            StoredObjectBase single = m_singleCache.Get(command);
            if (single != null)
            {
                return (T)single;
            }
            return null;
        }

        protected override void ClearCache()
        {
            m_groupCache.Clear();
            m_collectionCache.Clear();
            m_multipleCache.Clear();
            m_singleCache.Clear();
        }

        protected override void Recache(IDataCommand command)
        {
            m_groupCache.Remove(command);
            m_collectionCache.Remove(command);
            m_multipleCache.Remove(command);
            m_singleCache.Remove(command);
        }

        protected override void StoreCollection(IDataCommand command, IDataAdapterCollection group)
        {
            m_collectionCache.Add(command, group);
        }

        protected override void StoreGroup(IDataCommand command, IDataAdapterCollectionGroup group)
        {
            m_groupCache.Add(command, group);
        }

        protected override void StoreMultiple<T>(IDataCommand command, IList<T> items)
        {
            List<StoredObjectBase> storedItems = new List<StoredObjectBase>(items);
            m_multipleCache.Add(command, storedItems);
        }

        protected override void StoreSingle<T>(IDataCommand command, T item)
        {
            m_singleCache.Add(command, item);
        }
    }
}
