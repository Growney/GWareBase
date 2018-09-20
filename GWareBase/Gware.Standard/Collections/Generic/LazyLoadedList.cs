using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Collections.Generic
{
    public class LazyLoadedList<LoadedFrom, LoadedItem>
    {
        public async Task<LoadedItem> GetItem(long index)
        {
            if (m_idArray == null)
            {
                throw new IndexOutOfRangeException();
            }
            if (m_items[index] == null)
            {
                m_items[index] = await m_loadItem(m_idArray[index]);
            }
            return m_items[index];

        }

        public LoadedFrom GetID(long index)
        {
            if (m_idArray == null)
            {
                throw new IndexOutOfRangeException();
            }

            return m_idArray[index];
        }

        public long Count => m_idArray?.Length ?? 0;

        private Func<LoadedFrom, Task<LoadedItem>> m_loadItem;
        private LoadedFrom[] m_idArray;
        private LoadedItem[] m_items;

        public LazyLoadedList(Func<LoadedFrom, Task<LoadedItem>> loadItem, IEnumerable<LoadedFrom> ids)
        {
            m_loadItem = loadItem;

            m_idArray = ids?.ToArray();
            m_items = new LoadedItem[Count];
        }
    }
}
