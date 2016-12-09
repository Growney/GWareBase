using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Filter
{
    public class SingleFilter<T> : Filter<T> where T:IComparable
    {
        private T m_filterValue;

        public T FilterValue
        {
            get { return m_filterValue; }
            set { m_filterValue = value; }
        }

        public SingleFilter(string member) : base(member) { }
        public SingleFilter() { }

        protected override bool OnWithinFilter(T value)
        {
            return value.CompareTo(m_filterValue) == 0;
        }
    }
}
