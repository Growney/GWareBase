using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Connection;
using Gware.Gaming.Common.Networking.GamePacket;

namespace Gware.Gaming.Common.Networking
{
    public class AuthenticatedGameClient : GameClient
    {
        public AuthenticatedGameClient(IPEndPoint server) : base(server)
        {

        }

        protected override void OnPacketReceived(INetClient client, IGamePacket packet)
        {
            base.OnPacketReceived(client, packet);
        }
    }
}
