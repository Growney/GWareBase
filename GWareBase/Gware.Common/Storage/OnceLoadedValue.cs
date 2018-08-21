using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public class OnceLoadedValue<T>
    {
        private object m_getLock = new object();
        private Func<T> m_load;
        public bool Loaded { get; private set; }
        public Func<T> Load
        {
            set
            {
                if(m_load == null)
                {
                    m_load = value;
                }
            }
        }
        public T Value
        {
            get
            {
                T value = default(T);
                if (!Loaded)
                {
                    lock (m_getLock)
                    {
                        if (!Loaded)
                        {
                            value = m_load();
                            Loaded = true;
                        }
                    }
                }
                return value;
            }
        }
        public OnceLoadedValue()
        {
        }

        

    }
}
