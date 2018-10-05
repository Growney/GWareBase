using Gware.Standard.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Storage
{
    public abstract class LoadedFromAdapterBase
    {

        public LoadedFromAdapterBase()
        {
        }
        public virtual Task LoadAsync(IDataAdapter adapter)
        {
            return Task.Factory.StartNew(() =>
            {
                Load(adapter);
            });
        }
        public virtual void Load(IDataAdapter adapter)
        {
            LoadFrom(adapter);
        }
        protected abstract void LoadFrom(IDataAdapter adapter);

        public static Task<T> LoadSingleAsync<T>(IDataAdapterCollection collection, int index) where T : LoadedFromAdapterBase, new()
        {
            return Task<T>.Factory.StartNew(() =>
            {
                return LoadSingle<T>(collection, index);
            });
            
        }
        public static Task<T> LoadSingleAsync<T>(IDataAdapterCollection collection) where T : LoadedFromAdapterBase, new()
        {
            return LoadSingleAsync<T>(collection, 0);
        }
        public static Task<List<T>> LoadAsync<T>(IDataAdapterCollection collection) where T : LoadedFromAdapterBase, new()
        {
            return Task.Factory.StartNew(() =>
            {
                return Load<T>(collection);
            });
        }
        public static Task<List<T>> LoadAsync<T>(IDataAdapterCollectionGroup collection, int index) where T : LoadedFromAdapterBase, new()
        {
            return LoadAsync<T>(collection.Collections[index]);
        }

        public static T LoadSingle<T>(IDataAdapterCollection collection, int index) where T : LoadedFromAdapterBase, new()
        {
            T retVal;
            if (collection.Adapters.Length > 0 && collection.Adapters.Length >= index)
            {
                retVal = new T();
                retVal.Load(collection.Adapters[index]);
            }
            else
            {
                retVal = default;
            }

            return retVal;
        }
        public static T LoadSingle<T>(IDataAdapterCollection collection) where T : LoadedFromAdapterBase, new()
        {
            return LoadSingle<T>(collection, 0);
        }
        public static List<T> Load<T>(IDataAdapterCollection collection) where T : LoadedFromAdapterBase, new()
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
        public static List<T> Load<T>(IDataAdapterCollectionGroup collection, int index) where T : LoadedFromAdapterBase, new()
        {
            return Load<T>(collection.Collections[index]);
        }

    }
}
