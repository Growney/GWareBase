using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public class PingResponse : GamePacketBase
    {
        public PingResponse()
        {

        }
        protected PingResponse(ushort packetID) : base(packetID)
        {
        }
    }
}
