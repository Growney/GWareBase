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
    public class UdpServer : KeyedUdpNetClient
    {
        

        public int Broadcast(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
