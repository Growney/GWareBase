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
         
        public GameClient(IPEndPoint server)
        {
            m_listener = new TrackedUdpNetClient(server,0);
            m_listener.OnPacketReceived += OnPacketReceived;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();

            m_builder.OnDataCompelted += OnDataCompelted;
        }


        public override void Start()
        {
            m_listener.StartListening();
            m_listener.Start();
            base.Start();
        }

        public override bool Stop(int timeout = 500)
        {
            m_listener.StopListening();
            m_listener.Stop();
            return base.Stop(timeout);
        }

        private void OnDataCompelted(IPEndPoint from,byte[] data)
        {
            InfoResponsePacket res = new InfoResponsePacket();
            res.FromBytes(data);

            Console.WriteLine(String.Format("Ping Completed: {0}ms", TimeSpan.FromTicks(m_stopWatch.ElapsedTicks - res.StopWatchTime).TotalMilliseconds));
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
