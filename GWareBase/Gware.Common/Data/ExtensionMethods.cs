using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        public static int GetFieldIndex(this DataTable table,string field)
        {
            return table.Columns.IndexOf(field);
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
        public static string GetString(this DataRow row,string defaultValue)
        {
            return row.GetString(0, defaultValue);
        }
        public static string GetString(this DataRow row, string fieldName,string defaultValue)
        {
            return row.GetString(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static bool GetBoolean(this DataRow row,bool defaultValue)
        {
            return row.GetBoolean(0, defaultValue);
        }
        public static bool GetBoolean(this DataRow row,string fieldName, bool defaultValue)
        {
            return row.GetBoolean(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static int GetInt(this DataRow row, int defaultValue)
        {
            return row.GetInt(0, defaultValue);
        }
        public static int GetInt(this DataRow row, string fieldName, int defaultValue)
        {
            return row.GetInt(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static float GetFloat(this DataRow row,float defaultValue)
        {
            return row.GetFloat(0, defaultValue);
        }
        public static float GetFloat(this DataRow row,string fieldName, float defaultValue)
        {
            return row.GetFloat(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static decimal GetDecimal(this DataRow row, decimal defaultValue)
        {
            return row.GetDecimal(0, defaultValue);
        }
        public static decimal GetDecimal(this DataRow row, string fieldName, decimal defaultValue)
        {
            return row.GetDecimal(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static long GetLong(this DataRow row, long defaultValue)
        {
            return row.GetLong(0, defaultValue);
        }
        public static long GetLong(this DataRow row,string fieldName, long defaultValue)
        {
            return row.GetLong(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static DateTime GetDateTime(this DataRow row, DateTime defaultValue)
        {
            return row.GetDateTime(0, defaultValue);
        }
        public static DateTime GetDateTime(this DataRow row,string fieldName, DateTime defaultValue)
        {
            return row.GetDateTime(row.Table.GetFieldIndex(fieldName), defaultValue);
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
        public static byte[] GetData(this DataRow row, byte[] defaultValue)
        {
            return row.GetData(0, defaultValue);
        }
        public static byte[] GetData(this DataRow row,string fieldName, byte[] defaultValue)
        {
            return row.GetData(row.Table.GetFieldIndex(fieldName), defaultValue);
        }

        public static T DeepCopy<T>(this T val)
        {
            T retVal = default(T);
            if (typeof(T).IsSerializable)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, val);
                    ms.Position = 0;
                    retVal = (T)formatter.Deserialize(ms);
                }
            }
            return retVal;
            
        }

    }
}
