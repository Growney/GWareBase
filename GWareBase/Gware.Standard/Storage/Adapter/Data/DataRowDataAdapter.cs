using Gware.Standard.Data;
using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Adapter.Data
{
    public class DataRowDataAdapter : DataAdapterBase
    {
        private System.Data.DataRow m_row;

        public DataRowDataAdapter(ICommandController controller, System.Data.DataRow row)
            : base(controller)
        {
            m_row = row;
        }

        public override IEnumerable<string> GetFields()
        {
            string[] retVal = new string[m_row.Table.Columns.Count];

            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = m_row.Table.Columns[i].ColumnName;
            }

            return retVal;
        }

        public override string GetValue(string fieldName, string defaultValue)
        {
            return m_row.GetString(fieldName, defaultValue);
        }

        public override byte[] GetValue(string fieldName, byte[] defaultValue)
        {
            return m_row.GetData(fieldName, defaultValue);
        }

        public override void SetValue(string field, IConvertible value)
        {
            m_row[field] = value;
        }
    }
}
