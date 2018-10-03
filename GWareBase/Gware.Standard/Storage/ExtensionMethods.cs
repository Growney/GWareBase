using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage
{
    public static class ExtensionMethods
    {

        public static System.Data.DataTable CreateTableData<T>(this IEnumerable<T> items, string fieldName = "ID")
        {
            System.Data.DataTable retVal = new System.Data.DataTable();

            retVal.Columns.Add(fieldName, typeof(T));

            foreach (T item in items)
            {
                System.Data.DataRow row = retVal.NewRow();
                row[fieldName] = item;
                retVal.Rows.Add(row);
            }

            return retVal;
        }

        public static long MaxID(this IEnumerable<IHasID> x)
        {
            long retVal = long.MinValue;
            foreach (IHasID item in x)
            {
                retVal = Math.Max(item.Id, retVal);
            }
            return retVal;
        }

        public static long MinID(this IEnumerable<IHasID> x)
        {
            long retVal = long.MaxValue;
            foreach (IHasID item in x)
            {
                retVal = Math.Min(item.Id, retVal);

            }
            return retVal;
        }

        public static Guid GetGuid(this ICreatesGuid item)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] encoded = System.Text.Encoding.ASCII.GetBytes(item.GetGuidString());
                byte[] hash = md5.ComputeHash(encoded);
                return new Guid(hash);
            }
        }

        public static void Save<T>(this IEnumerable<T> list, ICommandController controller) where T : StoredObjectBase
        {
            list.Save(controller, null);
        }
        public static void Save<T>(this IEnumerable<T> list, ICommandController controller, Action<T> beforeSave = null) where T : StoredObjectBase
        {
            foreach (T item in list)
            {
                beforeSave?.Invoke(item);
                item.Save(controller);
            }
        }
    }
}
