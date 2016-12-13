using Gware.Common.Networking.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Gware.Common.DataStructures;
using Gware.Common.Delegates;
using Gware.Common.Networking.Packet;

namespace Gware.Common.Networking.FramedConnection
{
    public class ConnectionFramer
    {
        private bool m_useNetworkOrder;
        private Dictionary<IPEndPoint, TransferPacketFramer> m_framers = new Dictionary<IPEndPoint, TransferPacketFramer>();
        private Dictionary<IPEndPoint, TransferDataPacket[]> m_receviedPackets = new Dictionary<IPEndPoint, TransferDataPacket[]>();

        public event SingleResult<IPEndPoint, byte[]> OnDataCompleted;

        public ConnectionFramer(IDataReceiver receiver,bool useNetworkOrder)
        {
            receiver.OnDataRecevied += receiver_OnDataRecevied;
            m_useNetworkOrder = useNetworkOrder;
        }
        public ConnectionFramer(INetClient receiver, bool useNetworkOrder)
        {
            receiver.OnDataRecevied += receiver_OnDataRecevied;
            m_useNetworkOrder = useNetworkOrder;
        }
        public ConnectionFramer(INetServer receiver, bool useNetworkOrder)
        {
            receiver.OnDataRecevied += receiver_OnDataRecevied;
            m_useNetworkOrder = useNetworkOrder;
        }

        private void receiver_OnDataRecevied(System.Net.IPEndPoint sender, byte[] result)
        {
            lock (m_framers)
            {
                if (!m_framers.ContainsKey(sender))
                {
                    m_framers.Add(sender, new TransferPacketFramer());
                }
            }
            List<TransferDataPacket> packets = m_framers[sender].AddBytes(result);
            if (packets.Count > 0)
            {
                lock (m_receviedPackets)
                {
                    if (!m_receviedPackets.ContainsKey(sender))
                    {
                        m_receviedPackets.Add(sender, new TransferDataPacket[packets[0].Header.PacketTotal]);
                    }
                }
                TransferDataPacket[] m_packetBuffer = m_receviedPackets[sender];
                byte[] completedData = null;
                lock (m_packetBuffer)
                {
                    for (int i = 0; i < packets.Count; i++)
                    {
                        Console.WriteLine(String.Format("Packet {0} of {1} Recevied", packets[i].Header.PacketNumber + 1, packets[i].Header.PacketTotal));
                        TransferDataPacket packet = packets[i];
                        m_packetBuffer[packet.Header.PacketNumber] = packet;
                    }

                    if (m_packetBuffer.ArrayFull())
                    {
                        completedData = TransferDataPacket.GetData(m_packetBuffer,m_useNetworkOrder);
                    }
                }
                if(completedData != null)
                {
                    m_receviedPackets[sender].Empty();
                    m_receviedPackets.Remove(sender);
                    if (OnDataCompleted != null)
                    {
                        OnDataCompleted(sender, completedData);
                    }
                }
            }
        }
    }
}
