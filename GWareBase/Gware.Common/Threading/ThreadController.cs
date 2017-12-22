using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Threading
{
    public class ThreadController : TimerThread
    {
        private int m_maxThreads = 10;

        public int MaxThreads
        {
            get { return m_maxThreads; }
            set
            {
                if (m_maxThreads > value)
                {

                }
                m_maxThreads = value; 
            }
        }
        private Queue<IExecuteable> m_jobQueue = new Queue<IExecuteable>();

        private List<ActionThread> m_allThreads = new List<ActionThread>();

        private List<ActionThread> m_inactiveThreads = new List<ActionThread>();
        private List<ActionThread> m_completedThreads = new List<ActionThread>();
        private List<ActionThread> m_garbageThreads = new List<ActionThread>();
        private List<ActionThread> m_activeThreads = new List<ActionThread>();

        public ThreadController(int maxThreads)
        {
            m_maxThreads = maxThreads;
        }

        private ActionThread GetNextAvaliableThread()
        {
            ActionThread retVal = null;
            for (int i = m_completedThreads.Count - 1; i >= 0; i++)
            {
                m_inactiveThreads.Add(m_completedThreads[i]);
                if (m_activeThreads.Contains(m_completedThreads[i]))
                {
                    m_activeThreads.RemoveAt(m_activeThreads.IndexOf(m_completedThreads[i]));
                }
                m_completedThreads.RemoveAt(i);
            }
            if (m_allThreads.Count > m_maxThreads)
            {
                while (m_inactiveThreads.Count > m_maxThreads && m_maxThreads >= 0)
                {
                    m_inactiveThreads.RemoveAt(m_inactiveThreads.Count - 1);
                }
            }
            if (m_inactiveThreads.Count > 0)
            {
                retVal = m_inactiveThreads[0];
                m_inactiveThreads.RemoveAt(0);
            }
            else
            {
                ActionThread newThread = CreateNewThread();
                retVal = newThread;
            }


            return retVal;
        }
        private ActionThread CreateNewThread()
        {
            ActionThread thread = new ActionThread();
            thread.OnThreadComplete += OnThreadComplete;
            m_allThreads.Add(thread);
            return thread;
        }

        private void OnThreadComplete(object sender, ActionThread e)
        {
            m_completedThreads.Add(e);
        }

     
        protected override void OneSecondPing()
        {
            ActionThread nextThread = GetNextAvaliableThread();
            if (nextThread != null)
            {
                if (m_jobQueue.Count > 0)
                {
                    m_activeThreads.Add(nextThread);
                    IExecuteable exe = m_jobQueue.Dequeue();
                    nextThread.DoWork(exe);
                }
                else
                {
                    m_inactiveThreads.Add(nextThread);
                }
            }
        }

        protected override void OnThreadExit()
        {
            for (int i = 0; i < m_allThreads.Count; i++)
            {
                m_allThreads[i].Cancel();
            }
            //Split these over two loops to give the threads time to cancel before we stop them
            for (int i = 0; i < m_allThreads.Count; i++)
            {
                m_allThreads[i].Stop(1000);
            }
        }

        private void ReduceThreads(int reduceTo)
        {

        }

        public void AddAction(IExecuteable executable)
        {
            m_jobQueue.Enqueue(executable);
        }
    }
}
