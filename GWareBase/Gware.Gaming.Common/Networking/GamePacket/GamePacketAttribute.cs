using Gware.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public class GamePacketAttribute : ClassIDAttribute
    {
        public GamePacketAttribute(int classID) : base(classID)
        {
        }
    }
}
