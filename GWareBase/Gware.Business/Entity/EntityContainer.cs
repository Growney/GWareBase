using Gware.Business.Commands;
using Gware.Business.Entity.Collection;
using Gware.Common.Application;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity
{
    public abstract class EntityContainer : EntityBase
    {
        public EntityContainer()
        {

        }

        protected override void OnLoadFrom(IDataAdapter adapter)
        {
        }

        public override IDataCommand CreateSaveCommand()
        {
            return EntityContainerCommandFactory.Save(Id, EntityTypeID);
        }
                
        protected override IDataCommand GetLoadSingleCommand(int id)
        {
            return EntityContainerCommandFactory.LoadSingle(id, EntityTypeID);
        }

        protected override IDataCommand GetLoadCommand()
        {
            return EntityContainerCommandFactory.Load(EntityTypeID);
        }
    }
}
