using Gware.Common.DataStructures;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Cache
{
    public class CachedCommandHashedDictonary<Value> : IHashedDictionary<IDataCommand, Value>
    {
        private Dictionary<int, HashedDictionary<IDataCommand, Value>> m_dictionary;

        public CachedCommandHashedDictonary()
        {
            m_dictionary = new Dictionary<int, HashedDictionary<IDataCommand, Value>>();
        }

        public void Add(IDataCommand key, Value value)
        {
            int hash = key.GetHashCode(true);
            
            if (!m_dictionary.ContainsKey(hash))
            {
                m_dictionary.Add(hash, new HashedDictionary<IDataCommand, Value>());
            }
            m_dictionary[hash].Add(key, value);
        }

        public void Clear()
        {
            m_dictionary.Clear();
        }

        public Value Get(IDataCommand key)
        {
            int hash = key.GetHashCode(true);

            Value retVal = default(Value);

            if (m_dictionary.ContainsKey(hash))
            {
                retVal = m_dictionary[hash].Get(key);
            }

            return retVal;
        }

        public void Remove(IDataCommand key)
        {
            int hash = key.GetHashCode(true);

            if (m_dictionary.ContainsKey(hash))
            {
                m_dictionary[hash].Clear();
            }
        }
    }
}
