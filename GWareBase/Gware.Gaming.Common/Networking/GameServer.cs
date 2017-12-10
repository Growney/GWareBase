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
            m_udpListener.OnDataCompelted += PacketReceived;

            m_tcpServer = new BuiltTcpNetServer(port);
            m_tcpServer.OnTrackedClientConnected += OnTcpClientConnected;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
        }
        private void PacketReceived(INetClient client, byte[] data)
        {
            IGamePacket packet = GamePacketHelper.CreateAndLoadPacket(data);
            if(packet != null)
            {
                if(packet is IRequiresResponse responseRequired)
                {
                    client.Send(responseRequired.CreateReponse().ToBytes());
                }
                OnPacketReceived(client, packet);
            }
        }

        protected virtual void OnPacketReceived(INetClient client,IGamePacket packet)
        {

        }

        protected virtual void OnTcpClientConnected(BuiltTcpNetClient obj)
        {
            obj.Start();
            obj.OnDataCompelted += PacketReceived;
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
