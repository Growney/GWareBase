using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class BuiltUdpNetServer : TrackedUdpNetServer
    {
        private Dictionary<IPEndPoint, BuiltUdpNetServerClient> m_clients = new Dictionary<IPEndPoint, BuiltUdpNetServerClient>();
        private Dictionary<IPEndPoint, ConnectionDataBuilder> m_builders = new Dictionary<IPEndPoint, ConnectionDataBuilder>();
        public event Action<INetClient, byte[]> OnDataCompelted;

        public BuiltUdpNetServer(int port) : base(port)
        {

        }

        public BuiltUdpNetServer(int port, int key) : base(port, key)
        {

        }

        public override void PacketReceived(IPEndPoint from, TransferDataPacket data)
        {
            GetBuilder(from).Add(data);
        }

        private void DataCompleted(IPEndPoint endPoint,byte[] data)
        {
            BuiltUdpNetServerClient client;
            lock (m_clients)
            {
                if (m_clients.ContainsKey(endPoint))
                {
                    client = m_clients[endPoint];
                }
                else
                {
                    client = new BuiltUdpNetServerClient(endPoint, this);
                    m_clients.Add(endPoint, client);
                }
            }
            OnDataCompelted?.Invoke(client, data);           

        }

        private ConnectionDataBuilder GetBuilder(IPEndPoint from)
        {
            lock (m_builders)
            {
                if (!m_builders.ContainsKey(from))
                {
                    ConnectionDataBuilder builder = new ConnectionDataBuilder(from);
                    builder.OnDataCompelted += DataCompleted;
                    m_builders.Add(from, builder);

                }
                return m_builders[from];
            }
        }
    }
}
