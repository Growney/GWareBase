using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public class BuiltTcpNetServer : TcpNetServer
    {
        public event Action<BuiltTcpNetClient> OnTrackedClientConnected;

        public BuiltTcpNetServer(int port)
            :base(port)
        {
        }
        public BuiltTcpNetServer(IPAddress address, int port)
            :base(address,port)
        {
        }
        public BuiltTcpNetServer(IPEndPoint endPoint)
            :base(endPoint)
        {
        }

        protected override void ClientConnected(TcpClient client)
        {
            OnTrackedClientConnected?.Invoke(new BuiltTcpNetClient(client));
        }
    }
}
