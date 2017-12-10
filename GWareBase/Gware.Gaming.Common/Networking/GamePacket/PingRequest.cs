using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    [GamePacket((int)BasePacketType.PingRequest)]
    public class PingRequest : GamePacketBase, IRequiresResponse
    {
        public IGamePacket CreateReponse()
        {
            PingResponse retVal = new PingResponse();
            retVal.StopWatchTime = StopWatchTime;
            return retVal;
        }
    }
}
