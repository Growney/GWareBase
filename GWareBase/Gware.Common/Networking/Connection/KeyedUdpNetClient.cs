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

        public event Action<IPEndPoint, BufferReader> OnKeyedDataReceived;

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
            int key = reader.ReadInt32();
            if (key == c_key)
            {
                KeyedDataReceived(from, reader);
            }
        }

        public override bool Send(IPEndPoint sendTo, byte[] data)
        {
            BufferWriter writer = new BufferWriter(false);
            writer.WriteInt32(c_key);
            writer.WriteBytes(data);
            return base.Send(sendTo, writer.GetBuffer());
        }

        public override bool Send(string address, int port, byte[] data)
        {
            BufferWriter writer = new BufferWriter(false);
            writer.WriteInt32(c_key);
            writer.WriteBytes(data);
            return base.Send(address, port, writer.GetBuffer());
        }

        protected virtual void KeyedDataReceived(IPEndPoint from,BufferReader data)
        {
            OnKeyedDataReceived?.Invoke(from, data);
        }
    }
}
