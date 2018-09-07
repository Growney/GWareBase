using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public class OnceLoadedValue<T>
    {
        private T m_value;
        private object m_getLock = new object();
        private Func<T> m_load;
        public bool Loaded { get; private set; }
        public Func<T> Load
        {
            set
            {
                if (m_load == null)
                {
                    m_load = value;
                }
            }
        }
        public T Value
        {
            get
            {
                if (!Loaded)
                {
                    lock (m_getLock)
                    {
                        if (!Loaded)
                        {
                            m_value = m_load();
                            Loaded = true;
                        }
                    }
                }
                return m_value;
            }
        }
        public OnceLoadedValue()
        {
        }

        public void Reset()
        {
            Loaded = false;
        }


    }
}
