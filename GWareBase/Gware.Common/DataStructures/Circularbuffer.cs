using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public class CircularBuffer<T>
    {
        private object m_lock = new object();
        private T[] m_buffer;
        private int m_readPointer;
        private int m_writePointer;
        private int m_unreadCount;
        
        public T this[int index]
        {
            get
            {
                return m_buffer[m_buffer.GetNextPointer(m_readPointer, index)];
            }
            set
            {
                m_buffer[m_buffer.GetNextPointer(m_readPointer, index)] = value;
            }
        }
       
        public int UnreadCount
        {
            get { return m_unreadCount; }
            set { m_unreadCount = value; }
        }


        public CircularBuffer(int bufferSize)
        {
            m_buffer = new T[bufferSize];
            m_readPointer = 0;
            m_writePointer = 0;
        }
        
        public void Write(T[] bytes)
        {
            lock (m_lock)
            {
                int bytesBeforeOverlap = Math.Min(m_buffer.Length - m_writePointer, bytes.Length);
                Array.Copy(bytes, 0, m_buffer, m_writePointer, bytesBeforeOverlap);
                int bytesAfterOverlap = bytes.Length - bytesBeforeOverlap;
                Array.Copy(bytes, bytesBeforeOverlap, m_buffer, 0, bytesAfterOverlap);

                m_unreadCount += bytes.Length;
                m_writePointer = m_buffer.GetNextPointer(m_writePointer, bytes.Length);
            }
        }
        public void Write(T val)
        {
            lock (m_lock)
            {
                m_buffer[m_writePointer] = val;
                m_unreadCount++;
                m_writePointer = m_buffer.GetNextPointer(m_writePointer);
            }
        }
        public T[] Peek(int count)
        {
            T[] retVal = new T[count];

            lock (m_lock)
            {
                int bytesBeforeOverlap = Math.Min(m_buffer.Length - m_readPointer, count);
                Array.Copy(m_buffer, m_readPointer, retVal, 0, bytesBeforeOverlap);
                int bytesAfterOverlap = count - bytesBeforeOverlap;
                Array.Copy(m_buffer, 0, retVal, bytesBeforeOverlap, bytesAfterOverlap);
            }
            return retVal;
        }
        public T[] Read(int count)
        {
            T[] retVal = Peek(count);

            lock (m_lock)
            {
                m_unreadCount -= count;
                m_readPointer = m_buffer.GetNextPointer(m_readPointer, count);
            }

            return retVal;
 
        }
        public T Read()
        {
            lock (m_lock)
            {
                T retVal = m_buffer[m_readPointer];
                m_unreadCount--;
                m_readPointer = m_buffer.GetNextPointer(m_readPointer);

                return retVal;
            }
        }
        
    }
}
