using Gware.Business.Entity;
using Gware.Business.Entity.Attributes;
using Gware.Common.Application;
using Gware.Common.Reflection;
using Gware.Common.Storage;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Application
{
    public class EntityCommandControllerApplicationBase : CommandControllerApplicationBase<ICommandController>
    {
        public EntityCommandControllerApplicationBase(ICommandController controller) : base(controller)
        {
            ClassFactory<EntityTypeAttribute,EntityBase>.InitialiseEntityTypes(new Assembly[] { Assembly.GetAssembly(typeof(EntityBase)) });
        }
        
    }
}
