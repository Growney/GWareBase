using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Packet;

namespace Gware.Common.Networking.Connection
{
    public class TrackedUdpNetClient : TransferDataPacketUdpNetClient
    {
        private IPEndPoint m_server;
        private ConnectionTracker m_tracker;
        public event Action<IPEndPoint, ushort> OnPacketLoss;

        public TrackedUdpNetClient(IPEndPoint server,int port)
            :base(port)
        {
            m_server = server;
            m_tracker = new ConnectionTracker(server,OnPacketLoss);
        }

        public TrackedUdpNetClient(int port, int key)
            :base(port,key)
        {

        }

        public override void PacketReceived(IPEndPoint from, TransferDataPacket data)
        {
            m_tracker.AckSequence(data.Header.Sequence, data.Header.Ack);

            base.PacketReceived(from, data);
        }


        public override bool Send(IPEndPoint sendTo, TransferDataPacket data)
        {
            data.Header.Sequence = m_tracker.GetNextSequence();
            return base.Send(sendTo, data);
        }
        
        public bool Send(byte[] data)
        {
            return Send(m_server, data);
        }
    }
}
