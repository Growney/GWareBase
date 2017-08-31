using Gware.Common.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.Connection
{
    public interface IDataReceiver
    {
        event SingleResult<IPEndPoint, byte[]> OnDataRecevied;

        void StartListening();
        void StopListening();
    }
}
