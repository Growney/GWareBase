using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public struct ConnectionTracker
    {
        public int RemoteSequence { get; private set; }
        public uint Ack { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public ConnectionTracker(IPEndPoint endPoint)
        {
            RemoteSequence = 0;
            Ack = 0;
            EndPoint = endPoint;
        }

        public void UpdateRemoteSequence(int remoteSequence)
        {
            if(remoteSequence - )
        }
    }
}