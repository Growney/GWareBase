using Gware.Common.Data;
using Gware.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public static class GamePacketHelper
    {
        public static IGamePacket CreateAndLoadPacket(byte[] data)
        {
            BufferReader reader = new BufferReader(data);
            int classID = reader.ReadInt32();
            IGamePacket packet = ClassFactory<GamePacketAttribute, IGamePacket>.CreateClass(classID);
            packet.FromBuffer(reader);
            return packet;
        }
    }
}
