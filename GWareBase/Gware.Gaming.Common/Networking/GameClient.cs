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
using System.Threading;

namespace Gware.Gaming.Common.Networking
{
    public class GameClient 
    {
        static GameClient()
        {
            ClassFactory<GamePacketAttribute, IGamePacket>.InitialiseEntityTypes();
        }

        private Queue<AutoResetEvent> m_eventQueue = new Queue<AutoResetEvent>();
        private Dictionary<int,AutoResetEvent> m_responseEvents = new Dictionary<int,AutoResetEvent>();
        private Dictionary<int, IGamePacket> m_responsePackets = new Dictionary<int, IGamePacket>();

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
            m_udpClient.OnDataCompelted += PacketReceived;

            m_tcpClient = new BuiltTcpNetClient(server);
            m_tcpClient.OnDataCompelted += PacketReceived;

            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
        }

        private void PacketReceived(INetClient client, byte[] data)
        {
            IGamePacket packet = GamePacketHelper.CreateAndLoadPacket(data);
            if (packet != null)
            {
                Action<GameClient, IGamePacket> callback = null;
                lock (m_responseEvents)
                {
                    if (m_responseEvents.ContainsKey(packet.PacketID))
                    {
                        m_responsePackets.Set(packet.PacketID, packet);
                        m_responseEvents[packet.PacketID].Set();
                        m_responseEvents.Remove(packet.PacketID);
                    }
                }
                callback?.Invoke(this, packet);
                OnPacketReceived(client,packet);
            }
        }

        protected virtual void OnPacketReceived(INetClient client,IGamePacket packet)
        {

        }

        public void Start()
        {
            m_udpClient.StartListening();
            m_udpClient.Start();
            m_tcpClient.Start();
        }

        public void Stop()
        {
            m_udpClient.StopListening();
            m_udpClient.Stop();
            m_tcpClient.Stop();
        }
        
       
        private void InitPacketForSend(IGamePacket packet)
        {
            packet.StopWatchTime = m_stopWatch.ElapsedTicks;
        }
        private Task<IGamePacket> CreateAwaitTask(int packetID,int timeout)
        {
            AutoResetEvent set = new AutoResetEvent(false);
            lock (m_responseEvents)
            {
                m_responseEvents.Set(packetID, set);
            }
            return Task<IGamePacket>.Factory.StartNew(() =>
            {
                //copy local variables to task
                IGamePacket retVal = null;
                AutoResetEvent resetEvent = set;
                int packet = packetID;
                int tout = timeout;

                set.WaitOne(tout);

                lock (m_eventQueue)
                {
                    m_eventQueue.Enqueue(set);
                }
                
                lock (m_responsePackets)
                {
                    if (m_responsePackets.ContainsKey(packet))
                    {
                        retVal = m_responsePackets[packet];
                        m_responsePackets.Remove(packet);
                    }
                }

                return retVal;
            });
        }
        private AutoResetEvent GetNextResetEvent()
        {
            AutoResetEvent retVal = null;
            lock (m_eventQueue)
            {
                if(m_eventQueue.Count > 0)
                {
                    retVal = m_eventQueue.Dequeue();
                }
            }
            if(m_eventQueue == null)
            {
                retVal = new AutoResetEvent(false);
            }
            return retVal;
        }
        private Task<IGamePacket> SendWithResponse(INetClient client,IGamePacket packet,int timeout)
        {
            InitPacketForSend(packet);
            Task<IGamePacket> task = CreateAwaitTask(packet.PacketID,timeout);
            client.Send(packet.ToBytes());
            return task;
        }
        private void Send(INetClient client,IGamePacket packet)
        {
            InitPacketForSend(packet);
            client.Send(packet.ToBytes());
        }

        public void SendInsuredPacket(IGamePacket packet)
        {
            Send(m_tcpClient, packet);
        }

        public Task<IGamePacket> SendInsuredPacketWithResponse(IRequiresResponse packet,int timeout)
        {
            return SendWithResponse(m_tcpClient, packet,timeout); 
        }

        public void Send(IRequiresResponse packet)
        {
            Send(m_udpClient, packet);
        }

        public Task<IGamePacket> SendWithResponse(IGamePacket packet,int timeout)
        {
            return SendWithResponse(m_udpClient,packet,timeout);
        }

        public Task<TimeSpan> Ping(int timeout)
        {
            return SendInsuredPacketWithResponse(new PingRequest(), timeout).ContinueWith<TimeSpan>(x =>
            {
                if (!x.IsFaulted)
                {
                    IGamePacket response = x.Result;
                    if (response != null)
                    {
                        return TimeSpan.FromTicks(StopWatchTime - response.StopWatchTime);
                    }
                    else
                    {
                        return TimeSpan.MaxValue;
                    }
                }
                return TimeSpan.MaxValue;
                
            });
        }

    }
}
