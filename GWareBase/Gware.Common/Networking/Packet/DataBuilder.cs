using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Packet
{
    public class DataBuilder
    {
        private ushort m_sequence;
        private List<TransferDataPacket> m_data = new List<TransferDataPacket>();
        private ulong m_packetFlag;
        private ulong m_completeMask;
        private bool m_complete;

        public event Action<int, List<TransferDataPacket>> OnCompleted;

        public void SetForID(ushort dataID,ushort packetCount)
        {
            m_complete = false;
            m_sequence = dataID;
            SetCompleteMask(packetCount);
            m_data.Clear();
        }

        private void SetCompleteMask(ushort packetCount)
        {
            ulong shifted = (1UL << packetCount - 1);
            m_completeMask = (shifted - 1 | shifted);
        }

        public void Add(TransferDataPacket packet)
        {
            if(packet.Header.Sequence == m_sequence)
            {
                m_data.Add(packet);
                m_packetFlag |= (1UL << packet.Header.PacketNumber);

                if((m_completeMask & m_packetFlag) == m_completeMask)
                {
                    m_complete = true;
                    OnCompleted?.Invoke(m_sequence, m_data);
                }
            }
        }
    }
}
