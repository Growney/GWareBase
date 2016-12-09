using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Filter
{
    public class WithinFilter<T> : Filter<T> where T : IComparable
    {
        private List<T> m_filterValues;

        public List<T> FilterValues
        {
            get { return m_filterValues; }
            set { m_filterValues = value; }
        }

        public WithinFilter(string member) : base(member) { }
        public WithinFilter() { }

        protected override bool OnWithinFilter(T value)
        {
            for (int i = 0; i < m_filterValues.Count; i++)
            {
                if (value.CompareTo(m_filterValues[i]) == 0)
                {
                    return true;
                }

            }
            return false;
        }
    }
}
