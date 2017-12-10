using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Networking.Connection;
using Gware.Gaming.Common.Networking.GamePacket;

namespace Gware.Gaming.Common.Networking
{
    public class AuthenticatedGameServer : GameServer
    {
        private Dictionary<string, string> m_users = new Dictionary<string, string>();

        public AuthenticatedGameServer(int port) : base(port)
        {

        }

        protected override void OnPacketReceived(INetClient client, IGamePacket packet)
        {
            base.OnPacketReceived(client, packet);
        }
    }
}
