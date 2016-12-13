using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public static class ListExtensionMethods
    {
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

        public static bool ArrayFull(this Array a)
        {
            lock (a)
            {
                int missingBytes = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    if (a.GetValue(i) == null)
                    {
#if DEBUG
                        missingBytes++;
#else
                        return false;
#endif
                       
                    }
                }
                Console.WriteLine(String.Format("Array Missing Bytes {0}", missingBytes));
                return missingBytes == 0;
            }
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
