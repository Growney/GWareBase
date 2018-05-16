using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using Gware.Common.Delegates;
using Gware.Common.DataStructures;

namespace Gware.Common.Networking.Connection
{
    public class TcpNetServer : Threading.ThreadBase
    {
        private TcpListener m_listener;

        private Queue<TcpClient> m_queuedClients = new Queue<TcpClient>();

        private event Action<TcpNetClient> OnClientConnected;

        public TcpNetServer(int port)
        {
            m_listener = new TcpListener(IPAddress.Any, port);
        }
        public TcpNetServer(IPAddress address,int port)
        {
            m_listener = new TcpListener(address, port);
        }
        public TcpNetServer(IPEndPoint endPoint)
        {
            m_listener = new TcpListener(endPoint);
        }
        
        protected override void ExecuteSingleThreadCycle()
        {
            while(m_queuedClients.Count > 0)
            {
                TcpClient client = m_queuedClients.Dequeue();
                if (client != null)
                {
                    ClientConnected(client);
                }
            }
        }

        protected virtual void ClientConnected(TcpClient client)
        {
            OnClientConnected?.Invoke(new TcpNetClient(client));
        }

        public override bool Stop(int timeout)
        {
            m_listener.Stop();
            return base.Stop(timeout);
        }
        public override void Start()
        {
            m_listener.Start();
            m_listener.BeginAcceptTcpClient(BeginAccept, this);
            base.Start();
        }
        private void BeginAccept(IAsyncResult ar)
        {
            lock (m_queuedClients)
            {
                m_queuedClients.Enqueue(m_listener.EndAcceptTcpClient(ar));
            }

            Trigger();
        }
        

       


   
    }
}
