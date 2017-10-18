using Gware.Common.Data;
using Gware.Common.Networking.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public interface IGamePacket
    {
        int TypeID { get; }

        ushort PacketID { get; }
        byte PacketTypePrefix { get; }
        long StopWatchTime { get; set; }

        byte[] ToBytes();
        void ToBuffer(BufferWriter writer);
        void FromBuffer(BufferReader reader);

        IGamePacket CreateResponse();
        
    }
}
