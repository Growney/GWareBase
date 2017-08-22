using Gware.Business.Entity;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Application
{
    public class EntityCommandControllerApplicationBase : CommandControllerApplicationBase<ICommandController>
    {
        public EntityCommandControllerApplicationBase(ICommandController controller) : base(controller)
        {
            EntityFactory.InitialiseEntityTypes();
        }
        
    }
}
