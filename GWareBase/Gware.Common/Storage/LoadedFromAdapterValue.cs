using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;

namespace Gware.Common.Storage
{
    public class LoadedFromAdapterValue<T> : LoadedFromAdapterBase where T : IConvertible
    {
        private T m_value;
        
        public T Value
        {
            get
            {
                return m_value;
            }
        }

        protected override void LoadFrom(IDataAdapter adapter)
        {
            m_value = adapter.GetValue("Value", default(T));
        }
    }
}
