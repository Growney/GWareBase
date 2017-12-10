using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public interface INetClient
    {
        IPEndPoint RemoteEndPoint { get; }
        bool Send(byte[] data);
        
    }
}
