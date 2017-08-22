using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public static class ExtensionMethods
    {
        public static List<To> Convert<From,To>(this List<From> items) where To : From
        {
            List<To> retVal = new List<To>();

            foreach (From item in items)
            {
                if(item is To)
                {
                    retVal.Add((To)item);
                }
            }

            return retVal;
        }

        public static void Insert(this IList data,int index, object item, bool expand)
        {
            if (expand)
            {
                if (index > 0)
                {
                    while (index > data.Count)
                    {
                        data.Add(null);
                    }
                }
            }
            data[index] = item;

        }
        public static void Merge<T>(this BindingList<T> listOne, BindingList<T> listTwo) where T : IComparable<T>
        {
            for (int i = 0; i < listTwo.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < listOne.Count; j++)
                {
                    if (listTwo[i].CompareTo(listOne[j]) == 0)
                    {
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    listOne.Add(listTwo[i]);
                }
            }
        }
        public static void Merge<T>(this IList<T> listOne, IList<T> listTwo) where T : IComparable<T>
        {
            for (int i = 0; i < listTwo.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < listOne.Count; j++)
                {
                    if (listTwo[i].CompareTo(listOne[j]) == 0)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    listOne.Add(listTwo[i]);
                }
            }
        }
        public static void Set<Key, Value>(this Dictionary<Key, Value> dic, Key key, Value value)
        {
            lock (dic)
            {
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, value);
                }
                else
                {
                    dic[key] = value;
                }
            }
        }
        public static Value Get<Key,Value>(this Dictionary<Key,Value> dic,Key key)
        {
            lock (dic)
            {
                Value retVal = default(Value);
                if (dic.ContainsKey(key))
                {
                    retVal = dic[key];
                }
                return retVal;
            }
        }
        public static void AddItem<Key,Value,CollectionType>(this Dictionary<Key, CollectionType> dic,Key key,Value value) where CollectionType : ICollection<Value>,new ()
        {
            lock (dic)
            {
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, new CollectionType());
                }

                dic[key].Add(value);
            }
        }

        public static void Set<Key, Value, CollectionType>(this Dictionary<Key, CollectionType> dic,int index, Key key, Value value) where CollectionType : IList, new()
        {
            lock (dic)
            {
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, new CollectionType());
                }

                dic[key].Insert(index,value,true);
            }
        }

        public static T Get<T>(this IList<T> data,int index)
        {
            T retVal = default(T);

            if(index > 0 && index < data.Count)
            {
                retVal = data[index];
            }

            return retVal;
        }

        public static bool ArrayFull(this Array a)
        {
            bool retVal = true;
            lock (a)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a.GetValue(i) == null)
                    {
                        retVal = false;
                        break;
                    }
                }
            }
            return retVal;
        }
        public static void Empty(this Array a)
        {
            lock (a)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    a.SetValue(null, i);
                }
            }
        }
        public static void PopulateWithRange(this IList<int> list,Range<int> range)
        {
            list.Clear();
            for (int i = range.ReverseStart; i <= range.ReverseEnd; i++)
            {
                list.Add(i);
            }
        }
        public static void PopulateWithRange(this IList<int> list, int start,int end)
        {
            PopulateWithRange(list, new Range<int>(start, end));
        }
    }
}
