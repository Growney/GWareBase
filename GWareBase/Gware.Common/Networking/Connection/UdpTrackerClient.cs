using Gware.Common.Data;
using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class UdpTrackerClient : KeyedUdpNetClient
    {
        private Dictionary<IPEndPoint, ConnectionTracker> m_connectionTrackers = new Dictionary<IPEndPoint, ConnectionTracker>();

        public UdpTrackerClient(int port, int key)
            :base(port,key)
        {

        }

        public UdpTrackerClient(int port)
            :base(port)
        {

        }

        protected override void OnKeyedDataReceived(IPEndPoint from, BufferReader data)
        {
            TransferDataPacket packet = new TransferDataPacket();
            packet.FromBuffer(data);
            if (packet.IsValid)
            {
                if (!m_connectionTrackers.ContainsKey(from))
                {
                    m_connectionTrackers.Add(from, new ConnectionTracker(from, OnPacketLoss));
                }
                m_connectionTrackers[from].UpdateRemoteSequence(packet.Header.Sequence);
                OnValidPacketReceived(from, packet);
            }

        }
        protected virtual void OnPacketLoss(IPEndPoint from, ushort sequence)
        {

        }
        protected virtual void OnValidPacketReceived(IPEndPoint from, TransferDataPacket packet)
        {

        }
    }
}
