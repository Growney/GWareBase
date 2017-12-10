using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Networking.GamePacket
{
    public interface IRequiresResponse : IGamePacket
    {
        IGamePacket CreateReponse();
    }
}
