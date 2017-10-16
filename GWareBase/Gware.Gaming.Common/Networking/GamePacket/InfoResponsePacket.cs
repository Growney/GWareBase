using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public class InfoResponsePacket : InfoRequestPacket
    {
        public InfoResponsePacket()
        {

        }
        public InfoResponsePacket(InfoRequestPacketType type,ushort packetID)
            : base(type, packetID)
        {

        }
    }
}
