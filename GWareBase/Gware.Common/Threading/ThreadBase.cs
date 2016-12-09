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

        private bool m_stop;
        private Thread m_workerThread;
        private EventWaitHandle m_pauseEvent;
        private EventWaitHandle m_stoppedEvent;
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
        public ThreadBase()
        {
            m_stop = false;
            m_workerThread = new Thread(new ThreadStart(DoWork));
            m_workerThread.Name = this.GetType().Name;
            m_pauseEvent = new EventWaitHandle(true, EventResetMode.ManualReset);
            m_stoppedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        private void DoWork()
        {
            OnThreadInit();
            ExecuteEntireThreadCycle();
            OnThreadExit();
            m_stoppedEvent.Set();
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
                m_pauseEvent.WaitOne();

                if (!m_stop) // to ensure that we havent stopped while we were paused
                {
                    m_lastThreadRun = DateTime.UtcNow;
                    ExecuteSingleThreadCycle();
                    Thread.Sleep(c_sleepTime);
                }
            }
        }
        protected virtual void OnThreadInit()
        {

        }
        protected virtual void OnThreadExit()
        {

        }
        public void Pause()
        {
            m_pauseEvent.Reset();
        }
        public void Resume()
        {
            m_pauseEvent.Set();
        }
        public void Start()
        {
            m_stop = false;
            m_workerThread.Start();
        }
        public virtual bool Stop(int timeout = c_stopTimeout)
        {
            m_stop = true;
            Resume();
            return m_stoppedEvent.WaitOne(timeout);
        }

    }
}
