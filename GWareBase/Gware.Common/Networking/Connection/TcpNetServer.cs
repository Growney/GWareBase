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
    public class TcpNetServer : Threading.ThreadBase, INetServer, IDataReceiver, IDisposable
    {
        private TcpListener m_listener;
        private bool m_createConnectionOnSend = false;

        public event SingleResult<IPEndPoint, byte[]> OnDataRecevied;
        public event SingleResultWithReturn<IPEndPoint, bool> OnClientConnected;
        private Dictionary<IPEndPoint, TcpNetClient> m_connectedClients = new Dictionary<IPEndPoint, TcpNetClient>();

        public int ConnectedClients
        {
            get { return m_connectedClients.Count; }
        }
        public bool CreateConnectionOnSend
        {
            get { return m_createConnectionOnSend; }
            set { m_createConnectionOnSend = value; }
        }
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

        protected override void OnThreadInit()
        {
            m_listener.Start();
        }
        protected override void ExecuteSingleThreadCycle()
        {
            if (m_listener.Pending())
            {
                TcpClient connectedClient = m_listener.AcceptTcpClient();
                bool acceptClient = false;
                if (OnClientConnected != null)
                {
                    OnClientConnected(this,connectedClient.Client.RemoteEndPoint as IPEndPoint,ref acceptClient);
                }

                if (acceptClient)
                {
                    TcpNetClient client = new TcpNetClient(connectedClient);
                    InitialiseClient(client);
                    m_connectedClients.Set(connectedClient.Client.RemoteEndPoint as IPEndPoint, client);
                }
            }
        }
        private void InitialiseClient(TcpNetClient client)
        {
            client.StartListening();
            client.OnDataRecevied += client_OnDataRecevied;
            client.OnDisconnected += client_OnDisconnected;
  
        }
        private void client_OnDisconnected(IPEndPoint sender, TcpNetClient result)
        {
            m_connectedClients.Remove(sender);
        }

        private void client_OnDataRecevied(IPEndPoint sender, byte[] result)
        {
            if (OnDataRecevied != null)
            {
                OnDataRecevied(sender, result);
            }
        }

        public override bool Stop(int timeout)
        {
            m_listener.Stop();

            return base.Stop(timeout);
        }

        public void Dispose()
        {
            try
            {
                m_listener.Stop();
                m_listener = null;
            }
            catch (Exception)
            {

            }
            
        }

        public bool Send(string address, int port, byte[] data)
        {
            IPEndPoint endPoint = GetIPEndPointFromHostName(address, port, false);
            return Send(endPoint, data);
            
            throw new NotImplementedException();
        }

        public bool Send(IPEndPoint sendTo, byte[] data)
        {
            if (m_connectedClients.ContainsKey(sendTo))
            {
                TcpNetClient client = m_connectedClients[sendTo];
                return client.Send(data);
            }
            else
            {
                if (m_createConnectionOnSend)
                {
                    TcpNetClient newClient = new TcpNetClient();
                    newClient.Connect(sendTo);
                    return newClient.Send(data);
                }
            }
            return false;
        }
        public bool Send(string address, int port, Packet.TransferDataPacket data)
        {
            return Send(address, port, data.ToBytes());
        }

        public bool Send(IPEndPoint sendTo, Packet.TransferDataPacket data)
        {
            return Send(sendTo, data.ToBytes());
        }
        public int Broadcast(byte[] data)
        {
            int retVal = 0;
            List<IPEndPoint> currentlyConnected = new List<IPEndPoint>(m_connectedClients.Keys);

            for (int i = 0; i < currentlyConnected.Count; i++)
            {
                if (Send(currentlyConnected[i], data))
                {
                    retVal++;
                }
            }
            return retVal;
        }

        public void StartListening()
        {
            Start();
        }

        public void StopListening()
        {
            Start();
        }

        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP)
        {
            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else if (throwIfMoreThanOneIP && addresses.Length > 1)
            {
                throw new ArgumentException(
                    "There is more that one IP address to the specified host.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], port); // Port gets validated here.
        }


   
    }
}
