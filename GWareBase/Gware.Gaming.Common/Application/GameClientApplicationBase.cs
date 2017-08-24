using Gware.Common.Application;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Gaming.Common.Application
{
    public class GameClientApplicationBase : CommandControllerApplicationBase
    {
        public GameClientApplicationBase(ICommandController controller) : base(controller)
        {

        }
    }
}
