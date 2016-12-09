using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using Gware.Common.DataStructures;

namespace Gware.Common.Networking.Connection
{
    public class UdpNetClient : INetServer, IDataReceiver, INetClient
    {
        private TimeSpan m_connectionTimeOut = TimeSpan.FromMinutes(1);
        private UdpClient m_baseClient;
        private Dictionary<IPEndPoint, DateTime> m_connectedClients = new Dictionary<IPEndPoint, DateTime>();
        private bool m_connected;

        public TimeSpan ConnectionTimeOut
        {
            get { return m_connectionTimeOut; }
            set { m_connectionTimeOut = value; }
        }
        public int ConnectedClients
        {
            get { return m_connectedClients.Count; }
        }
        public bool Connected
        {
            get
            {
                return m_connected;
            }
        }

        public event Gware.Common.Delegates.SingleResultWithReturn<IPEndPoint, bool> OnClientConnected;
        public event Gware.Common.Delegates.SingleResult<IPEndPoint, byte[]> OnDataRecevied;

        public UdpNetClient()
        {
            m_baseClient = new UdpClient();
        }
        public UdpNetClient(int port,AddressFamily addrFamily)
        {
            m_baseClient = new UdpClient(port, addrFamily);
        }
        public UdpNetClient(int port)
        {
            m_baseClient = new UdpClient(port);
        }
        public UdpNetClient(AddressFamily addrFamily)
        {
            m_baseClient = new UdpClient(addrFamily);
        }

        private async Task ClearTimedOutClients()
        {
            List<IPEndPoint> keysToRemove = new List<IPEndPoint>();
            foreach (IPEndPoint key in m_connectedClients.Keys)
            {
                if ((DateTime.UtcNow - m_connectedClients[key]) > m_connectionTimeOut)
                {
                    keysToRemove.Add(key);
                }
            }
            foreach (IPEndPoint key in keysToRemove)
            {
                m_connectedClients.Remove(key);
            }
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
            data.Header.DateTimeObject = DateTime.UtcNow;
            return Send(address, port, data.ToBytes());
        }

        public bool Send(IPEndPoint sendTo, Packet.TransferDataPacket data)
        {
            data.Header.DateTimeObject = DateTime.UtcNow;
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

        public void Connect(IPEndPoint endPoint)
        {
            m_connected = true;
            m_baseClient.Connect(endPoint);
        }
        public void Connect(string address, int port)
        {
            m_connected = true;
            m_baseClient.Connect(address, port);
        }
        public void Connect(IPAddress address, int port)
        {
            m_connected = true;
            m_baseClient.Connect(address, port);
        }

        public void Disconnect()
        {
            StopListening();
        }

        public bool Send(byte[] data)
        {
            Task<int> task = m_baseClient.SendAsync(data, data.Length);            

            task.Wait();
            return true;
        }

        public bool Send(Packet.TransferDataPacket data)
        {
            data.Header.DateTimeObject = DateTime.UtcNow;
            return Send(data.ToBytes());
        }
        public void StartListening()
        {
            m_baseClient.BeginReceive(Receive,this);
        }
        private void Receive(IAsyncResult ar)
        {
            IPEndPoint from = null;

            Byte[] rxData = m_baseClient.EndReceive(ar, ref from);

            Task.Run(() =>
            {
                if (from != null)
                {
                    bool acceptClientData = false;
                    lock (m_connectedClients)
                    {
                        if (OnClientConnected != null && !m_connectedClients.ContainsKey(from))
                        {
                            OnClientConnected(this, from, ref acceptClientData);
                        }
                    }
                    
                    if (acceptClientData)
                    {
                        //clearClientsTask.Wait();
                        m_connectedClients.Set(from, DateTime.UtcNow);
                        if (OnDataRecevied != null)
                        {
                            OnDataRecevied(from, rxData);
                        }
                    }
                }
            });

            m_baseClient.BeginReceive(Receive, this);

        }
        public void StopListening()
        {
            m_baseClient.Close();
        }

        



        
    }
}
