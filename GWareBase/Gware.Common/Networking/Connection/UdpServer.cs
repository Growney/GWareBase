using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Data;
using Gware.Common.Delegates;
using Gware.Common.Networking.Packet;

namespace Gware.Common.Networking.Connection
{
    public class UdpServer : KeyedUdpNetClient,INetServer
    {
        public int ConnectedClients => throw new NotImplementedException();

        public UdpServer(int port, int key)
            :base(port,key)
        {

        }

        public UdpServer(int port)
            :base(port)
        {

        }

        public event SingleResultWithReturn<IPEndPoint, bool> OnClientConnected;
        public event SingleResult<IPEndPoint, byte[]> OnDataRecevied;

        protected override void OnKeyedDataReceived(IPEndPoint from, BufferReader data)
        {
            TransferDataPacket packet = new TransferDataPacket();
            packet.FromBuffer(data);
            //TODO sequence checking and acking


        }

        public int Broadcast(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
