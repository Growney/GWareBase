using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gware.Standard.Storage.Adapter.Data
{
    public class DataTableDataAdapter : DataAdapterCollectionBase<System.Data.DataTable>
    {
        public DataTableDataAdapter(ICommandController controller, DataTable loadFrom) : base(controller, loadFrom)
        {
        }

        protected override void OnLoadFrom(DataTable loadFrom)
        {
            int reqCount = loadFrom.Rows.Count;
            Adapters = new IDataAdapter[reqCount];
            if (loadFrom.Rows.Count > 0)
            {
                for (int i = 0; i < reqCount; i++)
                {
                    Adapters[i] = new DataRowDataAdapter(Controller, loadFrom.Rows[i]);
                }
            }
        }
    }
}
