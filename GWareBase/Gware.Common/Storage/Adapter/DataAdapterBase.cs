using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public abstract class DataAdapterBase : IDataAdapter
    {
        public abstract string GetValue(string fieldName, string defaultValue);
        public abstract byte[] GetValue(string fieldName, byte[] defaultValue);

        private T GetGenericValue<T>(string fieldName,T defaultValue) where T : IConvertible
        {
            T retVal = defaultValue;
            try
            {
                IConvertible value;
                if (defaultValue != null)
                {
                   value = GetValue(fieldName, defaultValue.ToString());
                }
                else
                {
                    value = GetValue(fieldName, string.Empty);
                }
                 
                retVal = (T)value.ToType(typeof(T), CultureInfo.CurrentCulture);
            }
            catch(Exception) 
            {

            }
            return retVal;
        }
        public sbyte GetValue(string fieldName,sbyte defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public short GetValue(string fieldName, short defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public int GetValue(string fieldName, int defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public long GetValue(string fieldName, long defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public decimal GetValue(string fieldName, decimal defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public float GetValue(string fieldName, float defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public double GetValue(string fieldName, double defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public byte GetValue(string fieldName, byte defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public ushort GetValue(string fieldName, ushort defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public uint GetValue(string fieldName, uint defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public ulong GetValue(string fieldName, ulong defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public char GetValue(string fieldName, char defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public bool GetValue(string fieldName, bool defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }
        public DateTime GetValue(string fieldName, DateTime defaultValue)
        {
            return GetGenericValue(fieldName, defaultValue);
        }

        public T GetValue<T>(string fieldName, T t) where T : IConvertible
        {
            return GetGenericValue<T>(fieldName, t);
        }

        
    }
}
