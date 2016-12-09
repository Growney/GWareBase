using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Gware.Common.Networking.Packet;

namespace Gware.Common.Networking.Connection
{
    public interface INetServer
    {
        int ConnectedClients
        {
            get;
        }

        event Gware.Common.Delegates.SingleResultWithReturn<IPEndPoint,bool> OnClientConnected;
        event Gware.Common.Delegates.SingleResult<IPEndPoint, byte[]> OnDataRecevied;

        bool Send(string address, int port, byte[] data);
        bool Send(IPEndPoint sendTo, byte[] data);
        bool Send(string address, int port, TransferDataPacket data);
        bool Send(IPEndPoint sendTo, TransferDataPacket data);
        int Broadcast(byte[] data);

        void StartListening();
        void StopListening();
    }
}
