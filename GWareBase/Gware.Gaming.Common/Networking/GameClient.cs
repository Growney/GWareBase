using Gware.Common.Networking.Connection;
using Gware.Common.Networking.Packet;
using Gware.Common.Threading;
using Gware.Gaming.Common.Networking.GamePacket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking
{
    public class GameClient : TimerThread
    {
        private Stopwatch m_stopWatch;
        private TrackedUdpNetClient m_listener;
        private TimeSpan m_lastSend = TimeSpan.Zero;

        private ConnectionDataBuilder m_builder = new ConnectionDataBuilder();
        private Dictionary<int, IGamePacket> m_packetHistory = new Dictionary<int, IGamePacket>();
         
        public GameClient(IPEndPoint server,int port)
        {
            m_listener = new TrackedUdpNetClient(server,port);
            m_listener.OnPacketReceived += OnPacketReceived;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();

            m_builder.OnDataCompelted += OnDataCompelted;
        }

        private void OnDataCompelted(IPEndPoint from,byte[] data)
        {
            
        }

        private void OnPacketReceived(IPEndPoint source, TransferDataPacket data)
        {
            m_builder.Add(data);
        }

        protected override void OneSecondPing()
        {
            TimeSpan elapsed = m_stopWatch.Elapsed;
            if((elapsed - m_lastSend).TotalMinutes > 0)
            {
                Send(new InfoRequestPacket(InfoRequestPacketType.Ping));
            }
        }

        public void Send(IGamePacket packet)
        {
            packet.StopWatchTime = m_stopWatch.ElapsedTicks;
            m_listener.Send(packet.ToBytes());
        }

    }
}
