using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.DataStructures
{
    public class Circularbuffer<T>
    {
        private object m_lock = new object();
        private T[] m_buffer;
        private int m_readPointer;
        private int m_writePointer;
        private int m_unreadBytes;
 
        public int UnreadBytes
        {
            get { return m_unreadBytes; }
            set { m_unreadBytes = value; }
        }


        public Circularbuffer(int bufferSize)
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

                m_unreadBytes += bytes.Length;
                m_writePointer = GetNextPointer(m_writePointer, bytes.Length);
            }
        }
        public void Write(T val)
        {
            lock (m_lock)
            {
                m_buffer[m_writePointer] = val;
                m_unreadBytes++;
                m_writePointer = GetNextPointer(m_writePointer);
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
                m_unreadBytes -= count;
                m_readPointer = GetNextPointer(m_readPointer, count);
            }

            return retVal;
 
        }
        public T Read()
        {
            lock (m_lock)
            {
                T retVal = m_buffer[m_readPointer];
                m_unreadBytes--;
                m_readPointer = GetNextPointer(m_readPointer);

                return retVal;
            }
        }
   
        private int GetNextPointer(int currentPointer,int add = 1)
        {
            return (currentPointer + add) % m_buffer.Length;
        }
    }
}
