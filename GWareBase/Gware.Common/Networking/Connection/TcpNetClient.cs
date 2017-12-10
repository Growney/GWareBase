using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using Gware.Common.Delegates;
using System.Net.NetworkInformation;

namespace Gware.Common.Networking.Connection
{
    public class TcpNetClient : Threading.ThreadBase
    {
        private TcpClient m_baseClient;
        private NetworkStream m_stream;
        private IPEndPoint m_remoteEndPoint;

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return m_remoteEndPoint;
            }
        }

        private Queue<byte[]> m_packetQueue = new Queue<byte[]>();
        
        public TcpNetClient(TcpClient client)
        {
            m_baseClient = client;
            m_remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
        }
        public TcpNetClient(IPEndPoint serverEndPoint)
        {
            m_baseClient = new TcpClient();
            m_remoteEndPoint = serverEndPoint;
        }
        protected override void ExecuteSingleThreadCycle()
        {
            byte[] data = null;
            lock (m_packetQueue)
            {
                if(m_packetQueue.Count > 0)
                {
                    data = m_packetQueue.Dequeue();
                }
            }

            if(data != null)
            {
                OnDataRecevied(data);
            }

            lock (m_packetQueue)
            {
                if(m_packetQueue.Count == 0)
                {
                    Pause();
                }
            }
        }

        protected virtual void OnDataRecevied(byte[] data)
        {

        }

        public override void Start()
        {
            if (!m_baseClient.Connected)
            {
                m_baseClient.Connect(m_remoteEndPoint);
            }
            m_stream = m_baseClient.GetStream();
            BeginRead();
            base.Start();
        }

        private void BeginRead()
        {
            byte[] buffer = new byte[1024];

            Task<int> readTask = m_stream.ReadAsync(buffer, 0, buffer.Length);

            readTask.ContinueWith(x =>
               {
                   if (!x.IsFaulted)
                   {
                       int receviedBytes = x.Result;
                       byte[] readBuffer = new byte[receviedBytes];
                       Buffer.BlockCopy(buffer, 0, readBuffer, 0, receviedBytes);
                       m_packetQueue.Enqueue(readBuffer);
                       Resume();
                       BeginRead();
                   }
               });
        }

        private void EndRead(IAsyncResult ar)
        {
            m_stream.EndRead(ar);
        }

        public void Disconnect()
        {
            m_stream.Close();
            m_baseClient.Close();
        }

        public virtual bool Send(byte[] bytes)
        {
            lock (m_stream)
            {
                if (m_stream.CanWrite)
                {
                    m_stream.Write(bytes, 0, bytes.Length);
                    return true;
                }
            }
            return false;
        }
        


        
    }
}
