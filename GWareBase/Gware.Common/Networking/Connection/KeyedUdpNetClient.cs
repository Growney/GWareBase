using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Reflection;
using Gware.Common.Data;

namespace Gware.Common.Networking.Connection
{
    public class KeyedUdpNetClient : UdpNetSender
    {
        private readonly int c_key;

        public KeyedUdpNetClient()
            :this(0)
        {

        }

        public KeyedUdpNetClient(int port)
            :this(port, Assembly.GetEntryAssembly().GetTitle().GetHashCode())
        {

        }

        public KeyedUdpNetClient(int port, int key)
            :base(port)
        {
            c_key = key;
        }

        protected override void OnDataReceived(IPEndPoint from, byte[] data)
        {
            BufferReader reader = new BufferReader(data);
            if(reader.ReadInt32() == c_key)
            {
                OnKeyedDataReceived(from, reader);
            }
        }

        protected virtual void OnKeyedDataReceived(IPEndPoint from,BufferReader data)
        {
            
        }
    }
}
