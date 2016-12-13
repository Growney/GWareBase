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
    public enum ClientServerConnectionType
    {
        UDP,
        TCP
    }
    public class FramedServer
    {
        private INetServer m_server;
        private ConnectionFramer m_framer;

        public FramedServer(int port, ClientServerConnectionType type,bool useNetworkOrder)
        {
            switch (type)
            {
                case ClientServerConnectionType.UDP:
                    m_server = new UdpNetClient(port);
                    break;
                case ClientServerConnectionType.TCP:
                    m_server = new TcpNetServer(port);
                    break;
                default:
                    throw new ArgumentException("Unsupported Client server type");
            }

            m_framer = new ConnectionFramer(m_server, useNetworkOrder);
            m_server.OnClientConnected += OnClientConnected;
            m_framer.OnDataCompleted += OnDataCompleted;
        }

        protected virtual void OnDataCompleted(IPEndPoint sender, byte[] result)
        {
            
        }

        protected virtual void OnClientConnected(object sender, System.Net.IPEndPoint result, ref bool retVal)
        {
            retVal = true;
        }

        protected virtual void Send(IPEndPoint to,byte[] bytes)
        {
            List<TransferDataPacket> m_toSend = TransferDataPacket.GetPackets(bytes);
            bool success = true;
            for (int i = 0; i < m_toSend.Count; i++)
            {
                success &=m_server.Send(to, m_toSend[i]);
            }
            
        }

        public void StartServer()
        {
            m_server.StartListening();
        }
        public void StopServer()
        {
            m_server.StopListening();
        }
    }
}
