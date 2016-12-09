using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Data
{
    public static class ExtensionMethods
    {
        public static int[] GetAllOrdinals(this DataTable table, Enum headers)
        {
            Array headerValues = Enum.GetValues(headers.GetType());
            int[] retVal = new int[headerValues.Length];
            for (int i = 0; i < headerValues.Length; i++)
            {
                retVal[i] = -1;
                string headerValueString = headerValues.GetValue(i).ToString();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Columns[j].ColumnName.Equals(headerValueString))
                    {
                        retVal[i] = j;
                        break;
                    }
                }
            }
            return retVal;
        }
        public static int[] GetAllOrdinals(this DataTable table, string[] headers)
        {
            int[] retVal = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                retVal[i] = -1;
                string headerValueString = headers[i];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Columns[j].ColumnName.Equals(headerValueString))
                    {
                        retVal[i] = j;
                        break;
                    }
                }
            }
            return retVal;
        }

        public static string GetString(this DataRow row, int index, string defaultValue)
        {
            string retVal = defaultValue;

            if (index >= 0 && index < row.Table.Columns.Count)
            {
                if(row[index] != DBNull.Value)
                {
                    retVal = row[index].ToString();
                }
            }

            return retVal;
        }
        public static bool GetBoolean(this DataRow row, int index, bool defaultValue)
        {
            string fieldValue = row.GetString(index, defaultValue.ToString());
            bool retVal;
            if (bool.TryParse(fieldValue, out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static int GetInt(this DataRow row, int index, int defaultValue)
        {
            string fieldValue = row.GetString(index, defaultValue.ToString());
            int retVal;
            if (Int32.TryParse(fieldValue, out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static float GetFloat(this DataRow row, int index, float defaultValue)
        {
            string fieldValue = row.GetString(index, defaultValue.ToString());
            float retVal;
            if (float.TryParse(fieldValue, out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static decimal GetDecimal(this DataRow row, int index, decimal defaultValue)
        {
            string fieldValue = row.GetString(index, defaultValue.ToString());
            decimal retVal;
            if (decimal.TryParse(fieldValue, out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static long GetLong(this DataRow row, int index, long defaultValue)
        {
            string fieldValue = row.GetString(index, defaultValue.ToString());
            long retVal;
            if (long.TryParse(fieldValue, out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static DateTime GetDateTime(this DataRow row, int index, DateTime defaultValue)
        {
            string fieldValue = row.GetString(index, defaultValue.ToString());
            DateTime retVal;
            if (DateTime.TryParse(fieldValue, out retVal))
            {
                return retVal;
            }
            return defaultValue; 
        }
        public static byte[] GetData(this DataRow row, int index, byte[] defaultValue)
        {
            byte[] retVal = defaultValue;
            if (index >= 0 && index < row.Table.Columns.Count)
            {
                if (row[index] != DBNull.Value)
                {
                    try
                    {
                        retVal = (byte[])row[index];
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return retVal;
        }

       
    }
}
