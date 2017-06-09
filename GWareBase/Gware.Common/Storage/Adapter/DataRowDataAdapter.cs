using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Data;
namespace Gware.Common.Storage.Adapter
{
    public class DataRowDataAdapter : DataAdapterBase
    {
        private System.Data.DataRow m_row;

        public DataRowDataAdapter(System.Data.DataRow row)
        {
            m_row = row;
        }

        public override string GetValue(string fieldName, string defaultValue)
        {
            return m_row.GetString(fieldName, defaultValue);
        }

        public override byte[] GetValue(string fieldName, byte[] defaultValue)
        {
            return m_row.GetData(fieldName, defaultValue);
        }
    }
}
