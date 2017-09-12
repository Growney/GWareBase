using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using Gware.Common.Delegates;
using System.Net.NetworkInformation;

namespace Gware.Common.Networking.Connection
{
    public class TcpNetClient : Threading.ThreadBase, INetClient, IDataReceiver, IDisposable
    {
        private TcpClient m_baseClient;
        private NetworkStream m_stream;
        private string m_outboundAddress;
        private int m_outboundPort;

        public event SingleResult<IPEndPoint, TcpNetClient> OnDisconnected;
        public event SingleResult<IPEndPoint, byte[]> OnDataRecevied;

        public bool Connected
        {
            get { throw new NotImplementedException(); }
        }
        public TcpNetClient(TcpClient client)
        {
            m_baseClient = client;
        }
        public TcpNetClient()
        {
            m_baseClient = new TcpClient();
        }
        public TcpNetClient(string outboundAddress,int outboundPort)
        {
            m_outboundAddress = outboundAddress;
            m_outboundPort = outboundPort;
        }
        private void Initialise()
        {
            if (!m_baseClient.Connected)
            {
                m_baseClient.Connect(m_outboundAddress, m_outboundPort);
            }
            if (m_baseClient != null)
            {
                m_stream = m_baseClient.GetStream();
            }
        }
        protected override void OnThreadInit()
        {
            Initialise();
        }
        protected override void ExecuteSingleThreadCycle()
        {
            TcpState connectionState = m_baseClient.GetState();
            if (connectionState == TcpState.Closed)
            {
                Stop();
                if (OnDisconnected != null)
                {
                    OnDisconnected(m_baseClient.Client.RemoteEndPoint as IPEndPoint, this);
                }
            }
            if (m_stream.DataAvailable)
            {
                byte[] rxBuffer = new byte[m_baseClient.ReceiveBufferSize];

                int receviedBytes = m_stream.Read(rxBuffer, 0, (int)m_baseClient.ReceiveBufferSize);

                byte[] dataBuffer = new byte[receviedBytes];

                Buffer.BlockCopy(rxBuffer, 0, dataBuffer, 0, receviedBytes);

                if (OnDataRecevied != null)
                {
                    OnDataRecevied(m_baseClient.Client.RemoteEndPoint as IPEndPoint, dataBuffer);
                }
            }
        }
        public void Connect(IPEndPoint endPoint)
        {
            m_baseClient.Connect(endPoint);
        }

        public void Connect(string address, int port)
        {
            m_baseClient.Connect(address, port);
        }

        public void Connect(IPAddress address, int port)
        {
            m_baseClient.Connect(address, port);
        }
        public void Disconnect()
        {
            m_stream.Close();
            m_baseClient.Close();
        }
        public void StartListening()
        {
            Start();
        }

        public void StopListening()
        {
            Stop();
        }
        public bool Send(byte[] bytes)
        {
            if (m_stream.CanWrite)
            {
                m_stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }
        public bool Send(Packet.TransferDataPacket data)
        {
            return Send(data.ToBytes());
        }
        public void Dispose()
        {
            if (m_stream != null)
            {
                m_stream.Close();
                m_stream.Dispose();
                m_stream = null;
            }
            if (m_baseClient != null)
            {
                m_baseClient.Close();
                m_baseClient = null;
            }
        }


        
    }
}
