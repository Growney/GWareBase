using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public static class ExtensionMethods
    {
        public static Dictionary<K, List<T>> GroupBy<K, T>(this IEnumerable<T> items, Func<T, K> groupBy)
        {
            Dictionary<K, List<T>> retval = new Dictionary<K, List<T>>();

            foreach (T item in items)
            {
                K key = groupBy(item);
                if (!retval.ContainsKey(key))
                {
                    retval.Add(key, new List<T>());
                }
                retval[key].Add(item);
            }
            return retval;
        }

        public static string ToDelimitedString<T>(this T flag, char limiter = ',') where T : Enum, IConvertible
        {
            return ToDelimitedString<T>((long)flag.ToInt64(System.Threading.Thread.CurrentThread.CurrentCulture), limiter);
        }
        public static string ToDelimitedString<T>(this long flag, char limiter = ',') where T : Enum, IConvertible
        {
            StringBuilder retVal = new StringBuilder();
            int count = 0;
            foreach (T enumVal in Enum.GetValues(typeof(T)))
            {
                long enumLong = enumVal.ToInt64(System.Threading.Thread.CurrentThread.CurrentCulture);
                if ((enumLong & flag) == enumLong)
                {
                    if (count > 0)
                    {
                        retVal.Append(",");
                    }
                    retVal.Append(enumVal.ToString());
                    count++;
                }
            }
            return retVal.ToString();
        }
        public static long ToFlag<T>(this IEnumerable<T> list) where T : Enum, IConvertible
        {
            long retVal = 0;

            foreach (T item in list)
            {
                retVal |= item.ToInt64(System.Threading.Thread.CurrentThread.CurrentCulture);
            }

            return retVal;
        }
        public static bool HasFlag<T>(this IEnumerable<T> list, T flag) where T : Enum, IConvertible
        {
            long flagLong = flag.ToInt64(System.Threading.Thread.CurrentThread.CurrentCulture);
            return (list.ToFlag() & flagLong) == flagLong;
        }
        public static IEnumerable<T> ToList<T>(this long flag) where T : Enum, IConvertible
        {
            List<T> retVal = new List<T>();
            foreach (T enumVal in Enum.GetValues(typeof(T)))
            {
                long enumLong = enumVal.ToInt64(System.Threading.Thread.CurrentThread.CurrentCulture);
                if ((enumLong & flag) == enumLong)
                {
                    retVal.Add(enumVal);
                }
            }

            return retVal;

        }
        public static List<To> Convert<From, To>(this List<From> items) where To : From
        {
            List<To> retVal = new List<To>();

            foreach (From item in items)
            {
                if (item is To)
                {
                    retVal.Add((To)item);
                }
            }

            return retVal;
        }
        public static Value Get<Key, Value>(this Dictionary<Key, Value> dic, Key key)
        {
            lock (dic)
            {
                Value retVal;
                if (dic.ContainsKey(key))
                {
                    retVal = dic[key];
                }
                else
                {
                    retVal = default(Value);
                }
                return retVal;
            }
        }
        public static void AddItem<Key, Value, CollectionType>(this Dictionary<Key, CollectionType> dic, Key key, Value value) where CollectionType : ICollection<Value>, new()
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
        public static void Set<Key, Value, CollectionType>(this Dictionary<Key, CollectionType> dic, int index, Key key, Value value) where CollectionType : IList, new()
        {
            lock (dic)
            {
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, new CollectionType());
                }

                dic[key].Insert(index, value, true);
            }
        }
        public static void Insert(this IList data, int index, object item, bool expand)
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

    }
}
