using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Packet
{
    public class ConnectionDataBuilder
    {
        private IPEndPoint m_from;
        private Dictionary<int, int> m_sequenceBuilder = new Dictionary<int, int>();
        private Dictionary<int, DataBuilder> m_builders = new Dictionary<int, DataBuilder>();
        private Queue<DataBuilder> m_freeBuilders = new Queue<DataBuilder>();

        public event Action<IPEndPoint,byte[]> OnDataCompelted;

        public ConnectionDataBuilder()
        {

        }

        public ConnectionDataBuilder(IPEndPoint from)
        {
            m_from = from;
        }

        public void Add(TransferDataPacket packet)
        {
            if (!m_sequenceBuilder.ContainsKey(packet.Header.Sequence))
            {
                DataBuilder builder = GetNextBuilder(packet.Header.Sequence, packet.Header.PacketTotal);
                builder.OnCompleted += OnCompleted;
                int builderHash = builder.GetHashCode(); 
                m_builders.Add(builderHash, builder);
                m_sequenceBuilder.Add(packet.Header.Sequence, builderHash);
            }

            m_builders[m_sequenceBuilder[packet.Header.Sequence]].Add(packet);
        }

        private void OnCompleted(int dataID, List<TransferDataPacket> packets)
        {
            if (m_sequenceBuilder.ContainsKey(dataID))
            {
                int builderHash = m_sequenceBuilder[dataID];
                if (m_builders.ContainsKey(builderHash))
                {
                    DataBuilder builder = m_builders[builderHash];
                    m_builders.Remove(builderHash);
                    m_freeBuilders.Enqueue(builder);
                }
            }

            OnDataCompelted?.Invoke(m_from,TransferDataPacket.GetData(packets,false));
        }

        private DataBuilder GetNextBuilder(ushort dataID,ushort packetCount)
        {
            DataBuilder retVal;
            if(m_freeBuilders.Count > 0)
            {
                retVal = m_freeBuilders.Dequeue();
            }
            else
            {
                retVal = new DataBuilder();
            }
            retVal.SetForID(dataID, packetCount);
            return retVal;
        }
    }
}
