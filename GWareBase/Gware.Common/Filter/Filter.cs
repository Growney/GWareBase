using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Filter
{
    public abstract class Filter<T> : IFilter where T : IComparable
    {
        private string m_member;

        public string Member
        {
            get { return m_member; }
            set { m_member = value; }
        }


        protected abstract bool OnWithinFilter(T value);

        public Filter(string member)
        {
            m_member = member;
        }
        public Filter()
        {
        }

        public bool WithinFilter(T value)
        {
            return OnWithinFilter(value);
        }

        public bool WithinFilter(object value)
        {
            try
            {
                Type valueType = value.GetType();
                PropertyInfo[] properties = valueType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.Equals(m_member))
                    {
                        object propertyValue = property.GetValue(value);
                        if (propertyValue != null)
                        {
                            if (propertyValue.GetType().Equals(typeof(T)))
                            {
                                T unboxedPropValue = (T)propertyValue;
                                return OnWithinFilter(unboxedPropValue);
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
