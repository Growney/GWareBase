using Gware.Common.Reflection;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public class MemoryDataAdapter : DataAdapterBase
    {
        

        private object m_object; 

        public MemoryDataAdapter(ICommandController controller,object item)
            :base(controller)
        {
            m_object = item;
        }

        public override IEnumerable<string> GetFields()
        {
            List<string> retVal = new List<string>();

            m_object.IteratePropertiesPerformAction((object performOn, PropertyInfo property) =>
            {
                retVal.Add(property.Name);
            });

            return retVal;
        }

        public override string GetValue(string fieldName, string defaultValue)
        {
            try
            {
                return m_object.GetType().GetProperty(fieldName).GetValue(m_object).ToString();
            }
            catch
            {
                return defaultValue;
            }
            
        }
        public override byte[] GetValue(string fieldName, byte[] defaultValue)
        {
            try
            {
                return (byte[])m_object.GetType().GetProperty(fieldName).GetValue(m_object);
            }
            catch
            {
                return defaultValue;
            }
        }
        public override void SetValue(string field, IConvertible value)
        {
            m_object.GetType().FindAndSetProperty(field, value.ToString());
        }

        
    }
}
