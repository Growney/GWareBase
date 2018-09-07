using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public class ArgumentsStore<T> : IArgumentsStore<T>
    {
        private Dictionary<Guid, T> m_store = new Dictionary<Guid, T>();

        public T ReCallArguments(string guid)
        {
            T retVal = default(T);
            try
            {
                if (Guid.TryParse(guid, out Guid newGuid))
                {
                    if (m_store.ContainsKey(newGuid))
                    {
                        retVal = m_store[newGuid];
                    }
                }
            }
            catch (Exception)
            {
            }
            return retVal;
        }

        public string StoreArguments(T parameters)
        {
            Guid paramGuid = Guid.NewGuid();

            if (!m_store.ContainsKey(paramGuid))
            {
                m_store.Add(paramGuid, parameters);
            }

            return paramGuid.ToString();
        }

        public void DiscardArguments(string guid)
        {
            if (Guid.TryParse(guid, out Guid newGuid))
            {
                if (m_store.ContainsKey(newGuid))
                {
                    m_store.Remove(newGuid);
                }
            }
        }
    }
}
