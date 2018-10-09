using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public class ArgumentsStore<T> : GuidArgumentStore<T>
    {
        private Dictionary<Guid, T> m_store = new Dictionary<Guid, T>();

        public ArgumentsStore(ILogger<ArgumentsStore<T>> logger)
            :base(logger)
        {

        }

        public override T RecallArguments(Guid guid)
        {
            T retVal = default(T);
            if (m_store.ContainsKey(guid))
            {
                retVal = m_store[guid];
            }
            return retVal;
        }

        public override void StoreArguments(Guid guid, T arguments)
        {
            if (!m_store.ContainsKey(guid))
            {
                m_store.Add(guid, arguments);
            }
        }

        public override bool DiscardArguments(Guid guid)
        {
            bool retVal = false;
            if (m_store.ContainsKey(guid))
            {
                m_store.Remove(guid);
                retVal = true;
            }
            return retVal;
        }
    }
}
