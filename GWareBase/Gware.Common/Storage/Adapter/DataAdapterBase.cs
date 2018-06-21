using Gware.Common.Storage.Command.Interface;
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
        
        public ICommandController Controller { get; private set; }
        public abstract string GetValue(string fieldName, string defaultValue);
        public abstract byte[] GetValue(string fieldName, byte[] defaultValue);

        public DataAdapterBase(ICommandController controller)
        {
            Controller = controller;
        }
        public T? GetValue<T>(string fieldName) where T : struct,IConvertible
        {
            T? retVal = null;
            try
            {
                IConvertible value;

                value = GetValue(fieldName, string.Empty);
                
                retVal = (T)value.ToType(typeof(T), CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {

            }
            return retVal;
        }

        public T GetValue<T>(string fieldName,T defaultValue) where T : IConvertible
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
        
        public abstract void SetValue(string field, IConvertible value);
        public abstract IEnumerable<string> GetFields();
    }
}
