using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public class StoredList<T> : List<T>, ISaveable where T : ISaveable
    {
        public StoredList(IEnumerable<T> val)
            :base(val)
        {

        }
        public long Save()
        {
            foreach (T val in this)
            {
                val.Save();
            }
            return 0;
        }
    }
}
