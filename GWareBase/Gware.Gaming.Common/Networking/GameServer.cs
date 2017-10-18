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
using Gware.Common.Reflection;
using Gware.Common.Data;

namespace Gware.Gaming.Common.Networking
{
    public class GameServer
    {
        static GameServer()
        {
            ClassFactory<GamePacketAttribute, IGamePacket>.InitialiseEntityTypes();
        }

        private Stopwatch m_stopWatch;
        private BuiltUdpNetServer m_udpListener;
        private BuiltTcpNetServer m_tcpServer;
        private List<BuiltTcpNetClient> m_tcpClients = new List<BuiltTcpNetClient>();

        public GameServer(int port)
        {
            m_udpListener = new BuiltUdpNetServer(port);
            m_udpListener.OnDataCompelted += OnUdpDataCompleted;

            m_tcpServer = new BuiltTcpNetServer(port);
            m_tcpServer.OnTrackedClientConnected += OnTcpClientConnected;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
        }

        protected virtual void OnTcpClientConnected(BuiltTcpNetClient obj)
        {
            obj.Start();
            obj.OnDataCompelted += OnTcpDataCompleted;
        }

        private void OnTcpDataCompleted(BuiltTcpNetClient sender,byte[] data)
        {
            IGamePacket packet = GamePacketHelper.CreateAndLoadPacket(data);
            if (packet != null)
            {
                IGamePacket response = packet.CreateResponse();
                sender.Send(response.ToBytes());
            }
        }

        
        protected virtual void OnUdpDataCompleted(IPEndPoint from, byte[] data)
        {
            IGamePacket packet = GamePacketHelper.CreateAndLoadPacket(data);
            if(packet != null)
            {
                IGamePacket response = packet.CreateResponse();
                m_udpListener.Send(from, response.ToBytes());
            }      

        }

        public void Start()
        {
            
            m_udpListener.StartListening();
            m_udpListener.Start();
            m_tcpServer.Start();

        }

        public bool Stop()
        {
            m_udpListener.StopListening();
            return m_udpListener.Stop();
        }


        
        
    }
}
