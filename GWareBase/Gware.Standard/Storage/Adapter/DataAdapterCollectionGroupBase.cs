using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Adapter
{
    public abstract class DataAdapterCollectionGroupBase<T> : IDataAdapterCollectionGroup
    {
        private IDataAdapterCollection[] m_collections;

        public IDataAdapterCollection[] Collections
        {
            get
            {
                return m_collections;
            }

            protected set
            {
                m_collections = value;
            }
        }
        public ICommandController Controller { get; private set; }
        public DataAdapterCollectionGroupBase(ICommandController controller, T loadFrom)
        {
            Controller = controller;
            OnLoadFrom(loadFrom);
        }

        protected abstract void OnLoadFrom(T loadFrom);

    }
}
