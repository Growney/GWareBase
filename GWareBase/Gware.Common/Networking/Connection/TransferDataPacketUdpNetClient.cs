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
    public class TransferDataPacketUdpNetClient : KeyedUdpNetClient
    {
        private object m_idLock = new object();
        private ushort m_dataID = 0;
        public event Action<IPEndPoint, TransferDataPacket> OnPacketReceived;

        public TransferDataPacketUdpNetClient(int port)
            :base(port)
        {

        }

        public TransferDataPacketUdpNetClient(int port, int key)
            :base(port,key)
        {

        }

        private ushort GetNextDataID()
        {
            lock (m_idLock)
            {
                m_dataID = unchecked((ushort)(m_dataID + 1));
            }

            return m_dataID;
        }

        protected override void KeyedDataReceived(IPEndPoint from, BufferReader data)
        {
            TransferDataPacket packet = new TransferDataPacket();
            packet.FromBuffer(data);
            if (!packet.IsValid)
            {
                base.KeyedDataReceived(from, data);
            }
            else
            {
                PacketReceived(from, packet);
            }
        }

        public virtual void PacketReceived(IPEndPoint from, TransferDataPacket data)
        {
            OnPacketReceived?.Invoke(from, data);
        }
        public override bool Send(string address, int port, byte[] data)
        {
            bool retVal = true;
            List<TransferDataPacket> packets = TransferDataPacket.GetPackets(data);
            for (int i = 0; i < packets.Count; i++)
            {
                retVal &= Send(address, port, packets[i]);
            }
            return retVal;

        }
        public override bool Send(IPEndPoint sendTo, byte[] data)
        {
            bool retVal = true;
            List<TransferDataPacket> packets = TransferDataPacket.GetPackets(data);
            for (int i = 0; i < packets.Count; i++)
            {
                retVal &= Send(sendTo, packets[i]);
            }
            return retVal;
        }

        public virtual bool Send(string address, int port, Packet.TransferDataPacket data)
        {
            return base.Send(address, port, data.ToBytes());
        }

        public virtual bool Send(IPEndPoint sendTo, Packet.TransferDataPacket data)
        {
            return base.Send(sendTo, data.ToBytes());
        }

        public virtual bool Send(string address, int port, IList<Packet.TransferDataPacket> packets)
        {
            bool retVal = true;
            for (int i = 0; i < packets.Count; i++)
            {
                retVal &= Send(address,port, packets[i]);
            }
            return retVal;
        }

        public virtual bool Send(IPEndPoint sendTo, IList<Packet.TransferDataPacket> packets)
        {
            bool retVal = true;
            for (int i = 0; i < packets.Count; i++)
            {
                retVal &= Send(sendTo, packets[i]);
            }
            return retVal;
        }
    }
}
