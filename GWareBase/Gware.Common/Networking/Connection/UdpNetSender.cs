using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using Gware.Common.DataStructures;
using Gware.Common.Threading;

namespace Gware.Common.Networking.Connection
{
    public class UdpNetSender : ThreadBase
    {
        private const int c_sleepTime = 0;
        private Queue<KeyValuePair<IPEndPoint, byte[]>> m_packetQueue = new Queue<KeyValuePair<IPEndPoint, byte[]>>();
        private System.Net.Sockets.UdpClient m_baseClient;
        
        public UdpNetSender()
            :this(0)
        {

        }
        public UdpNetSender(int port)
            : base(c_sleepTime)
        {
            m_baseClient = new System.Net.Sockets.UdpClient(port);
        }

        protected override void ExecuteSingleThreadCycle()
        {
            while(m_packetQueue.Count > 0 && !m_stop)
            {
                KeyValuePair<IPEndPoint, byte[]> data = m_packetQueue.Dequeue();
                OnDataReceived(data.Key, data.Value);
            } 
        }

        protected virtual void OnDataReceived(IPEndPoint from, byte[] data)
        {

        }

        public virtual bool Send(IPEndPoint sendTo, byte[] data)
        {
            lock (m_baseClient)
            {
                return (m_baseClient.Send(data, data.Length, sendTo) == data.Length);
            }
            
        }
        
        
        public async Task<bool> SendAyncAsync(byte[] data)
        {
            int sent = await m_baseClient.SendAsync(data, data.Length);            
            
            return sent == data.Length;
        }
        
        public void StartListening()
        {
            m_baseClient.BeginReceive(Receive,this);
        }
        private void Receive(IAsyncResult ar)
        {
            //TODO Resolve issues with connections closing
            try
            {
                IPEndPoint from = null;

                Byte[] rxData = m_baseClient.EndReceive(ar, ref from);

                lock (m_packetQueue)
                {
                    m_packetQueue.Enqueue(new KeyValuePair<IPEndPoint, byte[]>(from, rxData));
                    Trigger();
                }


                m_baseClient.BeginReceive(Receive, this);
            }
            catch (Exception ex)
            {

            }
            

        }
        public void StopListening()
        {
            m_baseClient.Close();
        }

    }
}
