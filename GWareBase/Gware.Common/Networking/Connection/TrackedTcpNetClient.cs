using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Packet;

namespace Gware.Common.Networking.Connection
{
    public class TrackedTcpNetClient : FramedTcpNetClient
    {
        private ConnectionTracker m_tracker = new ConnectionTracker();

        public TrackedTcpNetClient(TcpClient client) : base(client)
        {

        }
        public TrackedTcpNetClient(IPEndPoint serverEndPoint) : base(serverEndPoint)
        {

        }

        protected override void OnPacketReceived(TransferDataPacket packet)
        {
            m_tracker.AckSequence(packet.Header.Sequence, packet.Header.Ack);
            base.OnPacketReceived(packet);
        }

        public override bool Send(TransferDataPacket data)
        {
            data.Header.Sequence = m_tracker.GetNextSequence();
            return base.Send(data);
        }

    }
}
