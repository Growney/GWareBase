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
    public class BuiltTcpNetClient : TrackedTcpNetClient,INetClient
    {
        private ConnectionDataBuilder m_builder = new ConnectionDataBuilder();

        public event Action<INetClient, byte[]> OnDataCompelted;

        public BuiltTcpNetClient(TcpClient client)
            :base(client)
        {
            m_builder.OnDataCompelted += DataCompelted;
        }
        public BuiltTcpNetClient(IPEndPoint serverEndPoint)
            :base(serverEndPoint)
        {
            m_builder.OnDataCompelted += DataCompelted;
        }

        private void DataCompelted(IPEndPoint arg1, byte[] data)
        {
            OnDataCompelted?.Invoke(this,data);
        }

        protected override void OnPacketReceived(TransferDataPacket packet)
        {
            m_builder.Add(packet);
        }

        public bool Send(byte[] data)
        {
            return base.Send(data);
        }
    }
}
