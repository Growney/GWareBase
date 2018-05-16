using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gware.Common.Threading
{
    public class ThreadBase
    {
        private const int c_sleepTime = 50;
        private const int c_stopTimeout = 500;

        private bool m_running;
        private int m_sleepTime;
        protected bool m_stop;
        private ManualResetEvent m_pauseEvent;
        private ManualResetEvent m_stoppedEvent;
        private DateTime m_lastThreadRun;

        public event EventHandler<ThreadBase> OnExecuteSingleThreadCycle;

        public TimeSpan TimeSpanSinceLastExecute
        {
            get
            {
                return DateTime.UtcNow - m_lastThreadRun;
            }
        }
        public long TicksSinceLastExecute
        {
            get
            {
                return (DateTime.UtcNow - m_lastThreadRun).Ticks;
            }
        }
        public bool Paused
        {
            get
            {
                return m_pauseEvent.WaitOne(0);
            }
        }
        public bool IsStopping
        {
            get
            {
                return m_stop;
            }
        }

        public bool IsRunning
        {
            get
            {
                return m_running;
            }
        }

        public ThreadBase()
            :this(c_sleepTime)
        {

        }

        public ThreadBase(int sleepTime)
        {
            m_sleepTime = sleepTime;
            m_stop = false;
            m_pauseEvent = new ManualResetEvent(true);
            m_stoppedEvent = new ManualResetEvent(false);
        }

        private void DoWork(object threadContext)
        {
            try
            {
                OnThreadInit();
                ExecuteEntireThreadCycle();
                OnThreadExit();
            }
            finally
            {
                m_stoppedEvent.Set();
                m_running = false;
            }
            
        }
        protected virtual void ExecuteSingleThreadCycle()
        {
            if (OnExecuteSingleThreadCycle != null)
            {
                OnExecuteSingleThreadCycle(this, this);
            }
        }
        /// <summary>
        /// Warning when overiding this method the class must handle the last thread run time to ensure thread health is up to date
        /// </summary>
        protected virtual void ExecuteEntireThreadCycle()
        {
            while (!m_stop)
            {
                if (m_pauseEvent.WaitOne(10000))
                {
                    m_pauseEvent.Reset();
                    if (!m_stop) // to ensure that we havent stopped while we were paused
                    {
                        m_lastThreadRun = DateTime.UtcNow;
                        ExecuteSingleThreadCycle();
                    }
                }
            }
        }

        protected bool Wait(int timeout)
        {
            bool retVal = m_pauseEvent.WaitOne(timeout);
            m_pauseEvent.Reset();
            return retVal;
        }
        protected virtual void OnThreadInit()
        {

        }
        protected virtual void OnThreadExit()
        {

        }
        public virtual void Start()
        {
            m_running = true;
            m_stop = false;
            ThreadPool.QueueUserWorkItem(DoWork);
        }

        protected void Trigger()
        {
            m_pauseEvent.Set();
        }

        public virtual bool Stop(int timeout = c_stopTimeout)
        {
            m_stop = true;
            m_pauseEvent.Set();
            return m_stoppedEvent.WaitOne(timeout);
        }
    }
}
