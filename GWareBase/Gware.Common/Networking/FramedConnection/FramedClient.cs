using Gware.Common.Networking.Connection;
using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.FramedConnection
{
    public class FramedClient
    {
        protected INetClient m_client;
        private ConnectionFramer m_framer;

        public FramedClient(ClientServerConnectionType type, bool useNetworkOrder)
        {
            switch (type)
            {
                case ClientServerConnectionType.UDP:
                    m_client = new UdpNetClient();
                    break;
                case ClientServerConnectionType.TCP:
                    m_client = new TcpNetClient();
                    break;
                default:
                   throw new ArgumentException("Unsupported Client server type");
            }
            m_framer = new ConnectionFramer(m_client, useNetworkOrder);
            m_framer.OnDataCompleted += OnDataCompleted;
        }

        protected virtual void OnDataCompleted(IPEndPoint sender, byte[] result)
        {
            
        }

        public void Connect(IPEndPoint endPoint)
        {
            m_client.Connect(endPoint);
            m_client.StartListening();
        }
        public void Connect(string address, int port)
        {
            m_client.Connect(address, port);
            m_client.StartListening();
        }
        public void Connect(IPAddress address, int port)
        {
            m_client.Connect(address, port);
            m_client.StartListening();
        }

        public virtual void Send(byte[] data)
        {
            List<TransferDataPacket> m_toSend = TransferDataPacket.GetPackets(data);
            bool success = true;
            for (int i = 0; i < m_toSend.Count; i++)
            {
                success &= m_client.Send(m_toSend[i]);
            }
        }

        public void Disconnect()
        {
            m_client.Disconnect();
            m_client.StopListening();
        }

    }
}
