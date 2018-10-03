using System;
using System.Collections.Generic;
using System.Text;
using Gware.Standard.Storage.Adapter;

namespace Gware.Standard.Storage
{
    public class LoadedFromAdapterBytes : LoadedFromAdapterBase
    {
        public byte[] Value { get; private set; }

        protected override void LoadFrom(IDataAdapter adapter)
        {
            Value = adapter.GetValue("Value", new byte[0]);
        }
    }

    public static class LoadedFromAdapterBytesExtension
    {
        public static List<byte[]> ToList(this IEnumerable<LoadedFromAdapterBytes> list)
        {
            List<byte[]> retVal = new List<byte[]>();
            foreach (LoadedFromAdapterBytes item in list)
            {
                retVal.Add(item.Value);
            }
            return retVal;
        }
    }
}
