using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public class DataSetDataAdapter : DataAdapterCollectionGroupBase<System.Data.DataSet>
    {
        public DataSetDataAdapter(System.Data.DataSet data)
            :base(data)
        {
        }

        protected override void OnLoadFrom(DataSet loadFrom)
        {
            int reqCount = loadFrom.Tables.Count;
            Collections = new IDataAdapterCollection[loadFrom.Tables.Count];

            if (loadFrom.Tables.Count > 0)
            {
                for (int i = 0; i < reqCount; i++)
                {
                    Collections[i] = new DataTableDataAdapter(loadFrom.Tables[i]);
                }
            }
        }
    }
}
