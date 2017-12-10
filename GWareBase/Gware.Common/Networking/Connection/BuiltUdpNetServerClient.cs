using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class BuiltUdpNetServerClient : INetClient
    {

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return m_address;
            }
        }
        private IPEndPoint m_address;
        private BuiltUdpNetServer m_server;

        public BuiltUdpNetServerClient(IPEndPoint address, BuiltUdpNetServer server)
        {
            m_address = address;
            m_server = server;
        }

        public bool Send(byte[] data)
        {
            return m_server.Send(m_address, data);
        }
    }
}
