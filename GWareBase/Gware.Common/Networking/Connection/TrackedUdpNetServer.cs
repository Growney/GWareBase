using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Packet;
using Gware.Common.DataStructures;
namespace Gware.Common.Networking.Connection
{
    public class TrackedUdpNetServer : TransferDataPacketUdpNetClient
    {
        private Dictionary<IPEndPoint, ConnectionTracker> m_clientTrackers = new Dictionary<IPEndPoint, ConnectionTracker>();

        public TrackedUdpNetServer(int port) : base(port)
        {
        }

        public TrackedUdpNetServer(int port, int key) : base(port, key)
        {
        }
        private ConnectionTracker GetTracker(IPEndPoint endPoint)
        {
            lock (m_clientTrackers)
            {
                if (!m_clientTrackers.ContainsKey(endPoint))
                {
                    m_clientTrackers.Add(endPoint, new ConnectionTracker(endPoint));
                }

                return m_clientTrackers[endPoint];
            }
        }
        public override void PacketReceived(IPEndPoint from, TransferDataPacket data)
        {
            if (!m_clientTrackers.ContainsKey(from))
            {
                m_clientTrackers.Add(from, new ConnectionTracker(from));
            }
            m_clientTrackers[from].UpdateRemoteSequence(data.Header.Sequence);
            base.PacketReceived(from, data);
        }

        public override bool Send(IPEndPoint sendTo, TransferDataPacket data)
        {
            data.Header.Sequence = GetTracker(sendTo).GetNextSequence();
            return base.Send(sendTo, data);
        }

    }
}
