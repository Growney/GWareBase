using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class FramedTcpNetClient : TcpNetClient
    {
        private TransferDataPacketFramer framer = new TransferDataPacketFramer();

        public FramedTcpNetClient(TcpClient client)
            :base(client)
        {
        }
        public FramedTcpNetClient(IPEndPoint serverEndPoint)
            :base(serverEndPoint)
        {

        }

        protected override void OnDataRecevied(byte[] data)
        {
            List<TransferDataPacket> packet = framer.AddBytes(data);
            for (int i = 0; i < packet.Count; i++)
            {
                OnPacketReceived(packet[i]);
            }
        }
        
        protected virtual void OnPacketReceived(TransferDataPacket packet)
        {

        }

        public virtual bool Send(Packet.TransferDataPacket data)
        {
            return base.Send(data.ToBytes());
        }

        public virtual bool Send(IList<Packet.TransferDataPacket> packets)
        {
            bool retVal = true;
            for (int i = 0; i < packets.Count; i++)
            {
                retVal &= Send(packets[i]);
            }
            return retVal;
        }
        
        public override bool Send(byte[] bytes)
        {
            bool retVal = true;
            List<TransferDataPacket> packets = TransferDataPacket.GetPackets(bytes);
            for (int i = 0; i < packets.Count; i++)
            {
                retVal &= Send(packets[i]);
            }
            return retVal;
        }
    }
}
