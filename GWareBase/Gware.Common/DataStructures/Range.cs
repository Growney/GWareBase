using Gware.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public struct Range<T> : IComparable<Range<T>> where T : IComparable<T> 
    {
        private T m_start;
        private T m_end;

        public T Start
        {
            get
            {
                return m_start;
            }

            set
            {
                m_start = value;
            }
        }
        public T End
        {
            get
            {
                return m_end;
            }

            set
            {
                m_end = value;
            }
        }
        public T ReverseStart
        {
            get
            {
                if (Reverse)
                {
                    return End;
                }
                else
                {
                    return Start;
                }
            }
        }
        public T ReverseEnd
        {
            get
            {
                if (Reverse)
                {
                    return Start;
                }
                else
                {
                    return End;
                }
            }
        }

        public bool Reverse
        {
            get
            {
                return m_start.CompareTo(m_end) > 0;
            }
        }

        public Range(T start,T end)
        {
            m_start = start;
            m_end = end;
        }

        public Range(T start)
        {
            m_start = start;
            m_end = start;
        }

        public int CompareTo(Range<T> other)
        {
            int retVal = m_start.CompareTo(other.Start);
            if(retVal == 0)
            {
                retVal = m_end.CompareTo(other.End);
            }
            return retVal;
        }

        public bool Contains(T item)
        {
            return m_start.CompareTo(item) <= 0 && m_end.CompareTo(item) >= 0;
        }

        public bool Overlaps(T start, T end)
        {
            return Overlaps(new Range<T>(start, end));
        }

        public bool Overlaps(Range<T> val)
        {
            return HelperMethods.Overlap<T>(ReverseStart, ReverseEnd, val.ReverseStart, val.ReverseEnd);
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Start.ToString(), End.ToString());
        }
    }
}
