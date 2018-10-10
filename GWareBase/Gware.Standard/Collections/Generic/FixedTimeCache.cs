using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gware.Standard.Collections.Generic
{
    
    public class FixedTimeCache<K,D> : ICache<K, D>
    {
        private object m_accessLock = new object();
        private Dictionary<K, D> m_internalCache = new Dictionary<K, D>();

        private Dictionary<K, DateTime> m_readTime = new Dictionary<K, DateTime>();
        private Queue<K> m_timeoutQueue = new Queue<K>();
        
        private Queue<(K,TaskCompletionSource<D>,eCacheOptions)> m_readQueue = new Queue<(K, TaskCompletionSource<D>, eCacheOptions)>();
        private Queue<(K, D, DateTime)> m_writeQueue = new Queue<(K, D, DateTime)>();

        private System.Threading.Thread m_processThread;
        private bool m_stop;
        private AutoResetEvent m_event = new AutoResetEvent(false);

        public TimeSpan KeepFor { get; set; } = TimeSpan.FromMinutes(1);
        public Func<K, Task<D>> Read { get; set; }

        private readonly ILogger<FixedTimeCache<K, D>> m_logger;

        public FixedTimeCache(ILogger<FixedTimeCache<K,D>> logger)
        {
            m_logger = logger;
            m_processThread = new System.Threading.Thread(new System.Threading.ThreadStart(Process));
            m_processThread.Start();

        }

        private void Process()
        {
            m_logger.LogInformation("Starting fixed time cache process thread");
            while (!m_stop)
            {
                m_event.WaitOne(1000);
                m_event.Reset();

                CheckTimeouts();
                CheckWriteQueue();
                CheckReadQueue();
            }
            int failed = FailWaitingReads();

            m_logger.LogInformation($"Stopping fixed time cache process thread forced to fail {failed} reads");
        }

        private int FailWaitingReads()
        {
            int failedCount = 0;
            while(m_readQueue.Count > 0)
            {
                (K key, TaskCompletionSource<D> source,_) = m_readQueue.Dequeue();
                source.SetException(new Exception("Cannot read from stopped cache"));
                failedCount++;
            }
            return failedCount;
        }
        private void CheckTimeouts()
        {
            if(m_timeoutQueue.Count > 0)
            {
                K key = m_timeoutQueue.Peek();
                if((DateTime.UtcNow - m_readTime[key]) > KeepFor)
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
                (K key, TaskCompletionSource<D> source,eCacheOptions options) = m_readQueue.Dequeue();
                switch (options)
                {
                    case eCacheOptions.Default:
                        {
                            await PerformDefault(key, source);
                        }
                        break;
                    case eCacheOptions.ForceReload:
                        {
                            await PerformForce(key, source);
                        }
                        break;
                    case eCacheOptions.OnlyStored:
                        {
                            PerformOnlyStored(key, source);
                        }
                        break;
                    default:
                        break;
                }
                
            }
        }
        protected virtual async Task PerformForce(K key,TaskCompletionSource<D> source)
        {
            D item = await Read?.Invoke(key);
            source.SetResult(item);
            m_writeQueue.Enqueue((key, item, DateTime.UtcNow));
            m_event.Set();
        }
        private bool PerformOnlyStored(K key, TaskCompletionSource<D> source)
        {
            if (m_internalCache.ContainsKey(key))
            {
                source.SetResult(m_internalCache[key]);
                return true;
            }
            else
            {
                source.SetResult(default);
                return false;
            }
        }
        private async Task PerformDefault(K key,TaskCompletionSource<D> source)
        {
            if (!m_internalCache.ContainsKey(key))
            {
                await PerformForce(key, source);
            }
            else
            {
                PerformOnlyStored(key, source);
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

        public async Task<D> GetItem(K key, eCacheOptions options = eCacheOptions.Default)
        {
            if(Read == null)
            {
                throw new InvalidOperationException("Read must be defined before items can be taken from the cache");
            }
            Stopwatch timeToRead = new Stopwatch();
            timeToRead.Start();
            TaskCompletionSource<D> task = new TaskCompletionSource<D>();
            m_readQueue.Enqueue((key, task,options));
            m_event.Set();
            D retVal = await task.Task;
            m_logger.LogTrace($"Read cache item for key {key} of type {typeof(K)} for data type {typeof(D)} in {timeToRead.ElapsedMilliseconds}ms");
            return retVal;
        }

        public void Dispose()
        {
            m_stop = true;
            m_processThread.Join();
        }
    }
}
