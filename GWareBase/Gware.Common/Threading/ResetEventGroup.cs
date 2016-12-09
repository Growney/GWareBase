using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gware.Common.Threading
{
    public class ResetEventGroup<T,K>
    {
        private Dictionary<T, ManualResetEvent> m_events = new Dictionary<T, ManualResetEvent>();
        private Dictionary<T, K> m_items = new Dictionary<T, K>();

        private bool m_initialState;

        public bool InitialState
        {
            get { return m_initialState; }
            set { m_initialState = value; }
        }

        public ResetEventGroup()
        {
            m_initialState = false;
        }
        public ResetEventGroup(bool initialState)
        {
            m_initialState = initialState;
        }


        public bool Set(T id)
        {
            ExistsCreate(id);
            return m_events[id].Set();
        }
        public bool Reset(T id,K item)
        {
            if (m_events.ContainsKey(id))
            {
                bool retVal = m_events[id].Reset();
                Remove(id);
                return retVal;
            }
            return false;
            
        }

        public bool WaitOne(T id)
        {
            ExistsCreate(id);
            return m_events[id].WaitOne();
        }

        public bool WaitOne(T id, int timeout)
        {
            ExistsCreate(id);
            return m_events[id].WaitOne(timeout);
        }

        public bool WaitOne(T id, TimeSpan timeout)
        {
            ExistsCreate(id);
            return m_events[id].WaitOne(timeout);
        }
        private void ExistsCreate(T id)
        {
            if (!m_events.ContainsKey(id))
            {
                m_events.Add(id, new ManualResetEvent(m_initialState));
            }
        }

        public void Remove(T id)
        {
            m_events.Remove(id);
        }
    }
}
