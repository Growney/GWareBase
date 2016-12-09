using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Filter
{
    public enum RangeFilterBoundaryRule
    {
        /// <summary>
        /// Does not include the boundary values
        /// </summary>
        Exclusive,
        /// <summary>
        /// Includes the from boundary
        /// </summary>
        FromOverlap,
        /// <summary>
        /// Includes the to boundary
        /// </summary>
        ToOverlap,
        /// <summary>
        /// Includes both boundarys
        /// </summary>
        Enclusive
    }
    public class RangeFilter<T> : Filter<T> where T : IComparable
    {
        private T m_from;
        private T m_to;

        private RangeFilterBoundaryRule m_boundaryRule = RangeFilterBoundaryRule.Enclusive;

        public RangeFilterBoundaryRule BoundaryRule
        {
            get { return m_boundaryRule; }
            set { m_boundaryRule = value; }
        }
        public T To
        {
            get { return m_to; }
            set { m_to = value; }
        }

        public T From
        {
            get { return m_from; }
            set { m_from = value; }
        }

        public RangeFilter(string member) : base(member)
        { 

        }
        public RangeFilter()
        {

        }
        protected override bool OnWithinFilter(T value)
        {
            switch (m_boundaryRule)
            {
                case RangeFilterBoundaryRule.Exclusive:
                    return value.CompareTo(From) > 0 && value.CompareTo(To) < 0;
                case RangeFilterBoundaryRule.FromOverlap:
                    return value.CompareTo(From) >= 0 && value.CompareTo(To) < 0;
                case RangeFilterBoundaryRule.ToOverlap:
                    return value.CompareTo(From) > 0 && value.CompareTo(To) <= 0;
                case RangeFilterBoundaryRule.Enclusive:
                    return value.CompareTo(From) >= 0 && value.CompareTo(To) <= 0;
                default:
                    return false;
            }
        }

    }
}
