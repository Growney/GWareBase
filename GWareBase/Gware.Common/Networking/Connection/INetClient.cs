using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Gware.Common.Networking.Packet;

namespace Gware.Common.Networking.Connection
{
    public interface INetClient : IDataReceiver
    {
        bool Connected
        {
            get;
        }

        void Connect(IPEndPoint endPoint);
        void Connect(string address, int port);
        void Connect(IPAddress address, int port);

        void Disconnect();

        bool Send(byte[] data);
        bool Send(TransferDataPacket data);

        
    }
}
