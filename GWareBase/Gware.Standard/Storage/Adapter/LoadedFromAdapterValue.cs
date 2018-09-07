using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Adapter
{
    public class LoadedFromAdapterValue<T> : LoadedFromAdapterBase where T : IConvertible
    {
        public T Value { get; private set; }

        protected override void LoadFrom(IDataAdapter adapter)
        {
            Value = adapter.GetValue("Value", default(T));
        }
    }

    public static class LoadedFromAdapterValueExtension
    {
        public static List<T> ToList<T>(this IEnumerable<LoadedFromAdapterValue<T>> list) where T : IConvertible
        {
            List<T> retVal = new List<T>();
            foreach (LoadedFromAdapterValue<T> item in list)
            {
                retVal.Add(item.Value);
            }
            return retVal;
        }
    }
}
