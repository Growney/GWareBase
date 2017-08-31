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
        private UdpClient m_baseClient;
        
        public UdpNetSender()
            :base(c_sleepTime)
        {
            m_baseClient = new UdpClient();
        }
        public UdpNetSender(int port,AddressFamily addrFamily)
            : base(c_sleepTime)
        {
            m_baseClient = new UdpClient(port, addrFamily);
        }
        public UdpNetSender(int port)
            : base(c_sleepTime)
        {
            m_baseClient = new UdpClient(port);
        }
        public UdpNetSender(AddressFamily addrFamily)
            : base(c_sleepTime)
        {
            m_baseClient = new UdpClient(addrFamily);
        }

        protected override void ExecuteSingleThreadCycle()
        {
            KeyValuePair<IPEndPoint, byte[]>? data = null;
            lock (m_packetQueue)
            {
                if(m_packetQueue.Count > 0)
                {
                    data = m_packetQueue.Dequeue();
                }
            }

            if (data != null)
            {
                OnDataReceived(data.Value.Key, data.Value.Value);
            }

            lock (m_packetQueue)
            {
                if(m_packetQueue.Count == 0)
                {
                    Pause();
                }
            }
        }

        protected virtual void OnDataReceived(IPEndPoint from, byte[] data)
        {

        }

        public bool Send(string address, int port, byte[] data)
        {
           return (m_baseClient.Send(data, data.Length, address, port) == data.Length);
        }
        public bool Send(IPEndPoint sendTo, byte[] data)
        {
            return (m_baseClient.Send(data, data.Length, sendTo) == data.Length);
        }
        public bool Send(string address, int port, Packet.TransferDataPacket data)
        {
            data.Header.DateTime = DateTime.UtcNow;
            return Send(address, port, data.ToBytes());
        }

        public bool Send(IPEndPoint sendTo, Packet.TransferDataPacket data)
        {
            data.Header.DateTime = DateTime.UtcNow;
            return Send(sendTo, data.ToBytes());
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
            IPEndPoint from = null;

            Byte[] rxData = m_baseClient.EndReceive(ar, ref from);

            lock (m_packetQueue)
            {
                m_packetQueue.Enqueue(new KeyValuePair<IPEndPoint, byte[]>(from, rxData));
                Resume();
            }

            
            m_baseClient.BeginReceive(Receive, this);

        }
        public void StopListening()
        {
            m_baseClient.Close();
        }

        



        
    }
}
