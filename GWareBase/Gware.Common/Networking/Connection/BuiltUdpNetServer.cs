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
        private Dictionary<IPEndPoint, ConnectionDataBuilder> m_builders = new Dictionary<IPEndPoint, ConnectionDataBuilder>();
        public event Action<IPEndPoint, byte[]> OnDataCompelted;

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

        private ConnectionDataBuilder GetBuilder(IPEndPoint from)
        {
            lock (m_builders)
            {
                if (!m_builders.ContainsKey(from))
                {
                    ConnectionDataBuilder builder = new ConnectionDataBuilder(from);
                    builder.OnDataCompelted += OnDataCompelted;
                    m_builders.Add(from, builder);

                }
                return m_builders[from];
            }
        }
    }
}
