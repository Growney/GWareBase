using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public abstract class DataAdapterCollectionBase<T> : IDataAdapterCollection
    {
        private IDataAdapter[] m_adapters;
        public IDataAdapter[] Adapters
        {
            get
            {
                return m_adapters;
            }
            protected set
            {
                m_adapters = value;
            }
        }

        public DataAdapterCollectionBase(T loadFrom)
        {
            OnLoadFrom(loadFrom);
        }

        protected abstract void OnLoadFrom(T loadFrom);
    }
}
