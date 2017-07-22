using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public class HashedDictionary<Key,Value> : IHashedDictionary<Key, Value>
    {
        private object m_editLock = new object();
        private Dictionary<int, List<KeyValuePair<Key, Value>>> m_dictionary;

        public HashedDictionary()
        {
            m_dictionary = new Dictionary<int, List<KeyValuePair<Key, Value>>>();
        }

        public Value Get(Key key)
        {
            Value retVal = default(Value);
            int hash = key.GetHashCode();
            lock (m_editLock)
            {
                if (m_dictionary.ContainsKey(hash))
                {
                    List<KeyValuePair<Key, Value>> data = m_dictionary[hash];
                    for (int i = 0; i < data.Count; i++)
                    {
                        KeyValuePair<Key, Value> item = data[i];
                        if (item.Key.Equals(key))
                        {
                            retVal = item.Value;
                            break;
                        }
                    }
                }
            }
            return retVal;
        }

        public void Add(Key key,Value value)
        {
            int hash = key.GetHashCode();

            lock (m_editLock)
            {
                List<KeyValuePair<Key, Value>> collection;
                lock (m_editLock)
                {
                    if (!m_dictionary.ContainsKey(hash))
                    {
                        m_dictionary.Add(hash, new List<KeyValuePair<Key, Value>>());
                    }
                    collection = m_dictionary[hash];
                }
                
                lock (collection)
                {
                    KeyValuePair<Key, Value>? item = null;
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i].Key.Equals(key))
                        {
                            item = collection[i];
                            break;
                        }
                    }

                    if (item != null)
                    {
                        collection.Remove(item.Value);
                    }
                }
                
                collection.Add(new KeyValuePair<Key, Value>(key, value));

            }

        }

        public void Remove(Key key)
        {
            int hash = key.GetHashCode();

            lock (m_editLock)
            {
                if (m_dictionary.ContainsKey(hash))
                {
                    List<KeyValuePair<Key, Value>> data = m_dictionary[hash];
                    for (int i = 0; i < data.Count; i++)
                    {
                        KeyValuePair<Key, Value> item = data[i];
                        if (item.Key.Equals(key))
                        {
                            data.Remove(item);
                            break;
                        }
                    }
                }
            }
            
        }

        public void Clear()
        {
            lock (m_editLock)
            {
                m_dictionary.Clear();
            }
        }

    }
}
