using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gware.Common.Networking.Connection;
using System.Diagnostics;
using System.Net;
using Gware.Common.Networking.Packet;
using Gware.Gaming.Common.Networking.GamePacket;

namespace Gware.Gaming.Common.Networking
{
    public class GameServer
    {
        private Stopwatch m_stopWatch;
        private TrackedUdpNetServer m_listener;

        private Dictionary<IPEndPoint, ConnectionDataBuilder> m_builders = new Dictionary<IPEndPoint, ConnectionDataBuilder>();

        public GameServer(int port)
        {
            m_listener = new TrackedUdpNetServer(port);
            m_listener.OnPacketReceived += OnPacketReceived;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
        }

        public void Start()
        {
            m_listener.StartListening();
            m_listener.Start();
        }

        public bool Stop()
        {
            m_listener.StopListening();
            return m_listener.Stop();
        }

        private ConnectionDataBuilder GetBuilder(IPEndPoint from)
        {
            lock (m_builders)
            {
                if (!m_builders.ContainsKey(from))
                {
                    ConnectionDataBuilder builder = new ConnectionDataBuilder(from);
                    builder.OnDataCompelted += OnDataCompleted;
                    m_builders.Add(from, builder);
                    
                }
                return m_builders[from];
            }
        }

        private void OnDataCompleted(IPEndPoint from,byte[] obj)
        {
            InfoRequestPacket packet = new InfoRequestPacket();
            packet.FromBytes(obj);

            InfoResponsePacket res = new InfoResponsePacket(packet.RequestType, packet.PacketID);
            res.StopWatchTime = packet.StopWatchTime;
            m_listener.Send(from, res.ToBytes());

        }

        private void OnPacketReceived(IPEndPoint from, TransferDataPacket data)
        {
            GetBuilder(from).Add(data);
        }
    }
}
