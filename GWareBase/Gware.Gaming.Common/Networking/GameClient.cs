using Gware.Common.Data;
using Gware.Common.Networking.Connection;
using Gware.Common.Networking.Packet;
using Gware.Common.Reflection;
using Gware.Common.Threading;
using Gware.Gaming.Common.Networking.GamePacket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.DataStructures;

namespace Gware.Gaming.Common.Networking
{
    public class GameClient : TimerThread
    {
        static GameClient()
        {
            ClassFactory<GamePacketAttribute, IGamePacket>.InitialiseEntityTypes();
        }

        private Dictionary<int, Action<GameClient, IGamePacket>> m_insuredPackets = new Dictionary<int, Action<GameClient, IGamePacket>>();

        private Stopwatch m_stopWatch;
        private BuiltUdpNetClient m_udpClient;
        private BuiltTcpNetClient m_tcpClient;

        public long StopWatchTime
        {
            get
            {
                return m_stopWatch.ElapsedTicks;
            }
        }

        private TimeSpan m_lastSend = TimeSpan.Zero;
        
        public GameClient(IPEndPoint server)
        {
            m_udpClient = new BuiltUdpNetClient(server,0);
            m_udpClient.OnDataCompelted += OnUdpDataCompleted;

            m_tcpClient = new BuiltTcpNetClient(server);
            m_tcpClient.OnDataCompelted += OnTcpDataCompleted;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
        }

        protected virtual void OnTcpDataCompleted(BuiltTcpNetClient sender, byte[] data)
        {
            IGamePacket packet = GamePacketHelper.CreateAndLoadPacket(data);
            if(packet != null)
            {
                if (m_insuredPackets.ContainsKey(packet.PacketID))
                {
                    m_insuredPackets[packet.PacketID](this,packet);
                }
            }
        }

        protected virtual void OnUdpDataCompleted(BuiltUdpNetClient sender, byte[] data)
        {
            IGamePacket packet = GamePacketHelper.CreateAndLoadPacket(data);
            if (packet != null)
            {

            }
        }

        public override void Start()
        {
            m_udpClient.StartListening();
            m_udpClient.Start();
            m_tcpClient.Start();
            base.Start();
        }

        public override bool Stop(int timeout = 500)
        {
            m_udpClient.StopListening();
            m_udpClient.Stop();
            m_tcpClient.Stop();
            return base.Stop(timeout);
        }
        
        protected override void OneSecondPing()
        {
            TimeSpan elapsed = m_stopWatch.Elapsed;
            if((elapsed - m_lastSend).TotalMinutes > 0)
            {
                Send(new InfoRequestPacket(InfoRequestPacketType.Ping));
            }
        }

        public void SendInsuredPacket(IGamePacket packet,Action<GameClient,IGamePacket> onReply)
        {
            packet.StopWatchTime = m_stopWatch.ElapsedTicks;
            m_insuredPackets.Set(packet.PacketID, onReply);
            m_tcpClient.Send(packet.ToBytes());
            
        }

        public void Send(IGamePacket packet)
        {
            packet.StopWatchTime = m_stopWatch.ElapsedTicks;
            m_udpClient.Send(packet.ToBytes());
        }

    }
}
