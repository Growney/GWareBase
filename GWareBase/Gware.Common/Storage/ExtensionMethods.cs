using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public static class ExtensionMethods
    {
        public static System.Data.DataTable CreateIDList(this IEnumerable<int> ids)
        {
            System.Data.DataTable retVal = new System.Data.DataTable();

            retVal.Columns.Add("ID", typeof(int));

            foreach(int id in ids)
            {
                System.Data.DataRow row = retVal.NewRow();
                row["ID"] = id;
                retVal.Rows.Add(row);
            }

            return retVal;
        }
        
        public static long MaxID(this IEnumerable<IHasID> x)
        {
            long retVal = long.MinValue;
            foreach(IHasID item in x)
            {
                retVal = Math.Max(item.Id, retVal);
            }
            return retVal;
        }

        public static long MinID(this IEnumerable<IHasID> x)
        {
            long retVal = long.MaxValue;
            foreach(IHasID item in x)
            {
                retVal = Math.Min(item.Id, retVal);

            }
            return retVal;
        }
    }
}