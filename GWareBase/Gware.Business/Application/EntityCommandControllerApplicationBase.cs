﻿using Gware.Business.Entity;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Application
{
    public class EntityCommandControllerApplicationBase : CommandControllerApplicationBase<IStoredObjectCommandController>
    {
        public EntityCommandControllerApplicationBase(IStoredObjectCommandController controller) : base(controller)
        {
            EntityFactory.InitialiseEntityTypes();
        }
        
    }
}
