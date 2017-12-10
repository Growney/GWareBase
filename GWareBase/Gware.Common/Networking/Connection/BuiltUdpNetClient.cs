using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class BuiltUdpNetClient : TrackedUdpNetClient,INetClient
    {
        private ConnectionDataBuilder m_builder = new ConnectionDataBuilder();

        

        public event Action<INetClient,byte[]> OnDataCompelted;

        public BuiltUdpNetClient(IPEndPoint server, int port)
            :base(server,port)
        {
            m_builder.OnDataCompelted += DataCompelted;
        }

        private void DataCompelted(IPEndPoint arg1, byte[] arg2)
        {
            OnDataCompelted?.Invoke(this,arg2);
        }

        public override void PacketReceived(IPEndPoint from, TransferDataPacket data)
        {
            m_builder.Add(data);
            base.PacketReceived(from, data);
        }

        public bool Send(byte[] data)
        {
            return base.Send(data);
        }
    }
}
