using Gware.Common.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Data;
using Gware.Common.Reflection;
using System.Reflection;

namespace Gware.Common.Storage
{
    public abstract class LoadedFromAdapterBase
    {
        private LoadedFromAdapterBase m_nonDirtyState;
        private bool m_gotDirty = false;
        private bool m_dirty;

        
        public LoadedFromAdapterBase()
        {
            
        }

        protected virtual bool GetIsDirty()
        {
            lock (this)
            {
                if (!m_gotDirty)
                {
                    m_gotDirty = true;
                    m_dirty = ChangedFromDirtyState();

                }
                return m_dirty;
            }
            
        }

        private bool ChangedFromDirtyState()
        {
            bool retVal = false;

            if(m_nonDirtyState == null)
            {
                SetNonDirtyState(Activator.CreateInstance(this.GetType()) as LoadedFromAdapterBase);
            }
            
            this.IteratePropertiesPerformAction(new Reflection.ExtensionMethods.ReflectionPropertyAction(delegate (object x, PropertyInfo y)
            {
                object nonDirtyPropertyValue = y.GetValue(m_nonDirtyState);
                object thisPropertyValue = y.GetValue(x);

                if(thisPropertyValue != null && nonDirtyPropertyValue != null)
                {
                    if (!thisPropertyValue.Equals(nonDirtyPropertyValue))
                    {
                        retVal = true;
                    }
                }
                else if(thisPropertyValue == null && nonDirtyPropertyValue != null)
                {
                    retVal = true;
                }
                else if(thisPropertyValue != null && nonDirtyPropertyValue == null)
                {
                    retVal = true;
                }
                    
            }));
            
            return retVal;
        }
        public void SetNonDirtyState(LoadedFromAdapterBase value)
        {
            lock (this)
            {
                m_nonDirtyState = value;
                m_gotDirty = false;
            }
        }
        
        public virtual void Load(IDataAdapter adapter)
        {
            LoadFrom(adapter);
            if(m_nonDirtyState != null)
            {
                m_nonDirtyState.Load(adapter);
            }
        }
        protected abstract void LoadFrom(IDataAdapter adapter);

        public static T LoadSingle<T>(IDataAdapterCollection collection,int index) where T : LoadedFromAdapterBase, new()
        {
            T retVal;
            if (collection.Adapters.Length > 0 && collection.Adapters.Length >= index)
            {
                retVal = new T();
                retVal.Load(collection.Adapters[index]);
            }
            else
            {
                retVal = default(T);
            }
            
            return retVal;
        }
        public static T LoadSingle<T>(IDataAdapterCollection collection) where T : LoadedFromAdapterBase, new()
        {
            return LoadSingle<T>(collection, 0);
        }

        public static List<T> Load<T>(IDataAdapterCollection collection) where T: LoadedFromAdapterBase,new()
        {
            List<T> retVal = new List<T>();

            for (int i = 0; i < collection.Adapters.Length; i++)
            {
                T item = new T();
                item.LoadFrom(collection.Adapters[i]);
                retVal.Add(item);
            }

            return retVal;
        }
        public static List<T> Load<T>(IDataAdapterCollectionGroup collection,int index) where T : LoadedFromAdapterBase, new()
        {
            return Load<T>(collection.Collections[index]);
        }

    }
}
