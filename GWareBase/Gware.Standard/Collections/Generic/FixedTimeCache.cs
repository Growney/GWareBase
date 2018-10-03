using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gware.Standard.Collections.Generic
{
    public class FixedTimeCache<K,D> : IDisposable
    {
        private object m_accessLock = new object();
        private Dictionary<K, D> m_internalCache = new Dictionary<K, D>();

        private Dictionary<K, DateTime> m_readTime = new Dictionary<K, DateTime>();
        private Queue<K> m_timeoutQueue = new Queue<K>();
        
        private Queue<(K,TaskCompletionSource<D>)> m_readQueue = new Queue<(K, TaskCompletionSource<D>)>();
        private Queue<(K, D, DateTime)> m_writeQueue = new Queue<(K, D, DateTime)>();

        private System.Threading.Thread m_processThread;
        private bool m_stop;
        private TimeSpan m_keepFor;
        private Func<K, Task<D>> m_read;
        private AutoResetEvent m_event = new AutoResetEvent(false);

        public FixedTimeCache(Func<K, Task<D>> read)
            :this(read,TimeSpan.FromMinutes(1))
        {

        }

        public FixedTimeCache(Func<K, Task<D>> read,TimeSpan keepFor)
        {
            m_processThread = new System.Threading.Thread(new System.Threading.ThreadStart(Process));
            m_processThread.Start();
            m_keepFor = keepFor;
            m_read = read;

        }

        private void Process()
        {
            while (!m_stop)
            {
                m_event.WaitOne(1000);
                m_event.Reset();

                CheckTimeouts();
                CheckWriteQueue();
                CheckReadQueue();
            }
            FailWaitingReads();
        }

        private void FailWaitingReads()
        {
            while(m_readQueue.Count > 0)
            {
                (K key, TaskCompletionSource<D> source) = m_readQueue.Dequeue();
                source.SetException(new Exception("Cannot read from stopped cache"));
            }
        }
        private void CheckTimeouts()
        {
            if(m_timeoutQueue.Count > 0)
            {
                K key = m_timeoutQueue.Peek();
                if((DateTime.UtcNow - m_readTime[key]) > m_keepFor)
                {
                    m_internalCache.Remove(key);
                    m_timeoutQueue.Dequeue();
                    m_readTime.Remove(key);
                }
            }
        }
        private async Task CheckReadQueue()
        {
            if(m_readQueue.Count > 0)
            {
                (K key, TaskCompletionSource<D> source) = m_readQueue.Dequeue();
                if (m_internalCache.ContainsKey(key))
                {
                    source.SetResult(m_internalCache[key]);
                }
                else
                {
                    D item = await m_read(key);
                    source.SetResult(item);
                    m_writeQueue.Enqueue((key, item, DateTime.UtcNow));
                    m_event.Set();
                    
                    
                }
            }
        }
        private void CheckWriteQueue()
        {
            while (m_writeQueue.Count > 0)
            {
                (K key, D data, DateTime checkedOn) = m_writeQueue.Dequeue();
                m_internalCache.Set(key, data);
                m_readTime.Set(key, DateTime.UtcNow);
                m_timeoutQueue.Enqueue(key);
            }
        }
        public Task<D> GetItem(K key)
        {
            TaskCompletionSource<D> task = new TaskCompletionSource<D>();

            m_readQueue.Enqueue((key, task));
            m_event.Set();
            return task.Task;
        }

        public void Dispose()
        {
            m_stop = true;
            m_processThread.Join();
        }
    }
}
