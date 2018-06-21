using Gware.Common.Storage.Command.Interface;
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
        public IDataAdapter First
        {
            get
            {
                if(m_adapters != null && m_adapters.Length > 0)
                {
                    return m_adapters[0];
                }
                return null;
            }
        }

        public ICommandController Controller { get; private set; }

        public DataAdapterCollectionBase(ICommandController controller,T loadFrom)
        {
            Controller = controller;
            OnLoadFrom(loadFrom);
        }

        protected abstract void OnLoadFrom(T loadFrom);
    }
}
