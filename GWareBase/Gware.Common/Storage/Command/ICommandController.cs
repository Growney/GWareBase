using Gware.Common.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command
{
    public interface ICommandController
    {
        IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command);
        IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command);
        int ExecuteQuery(IDataCommand command);
    }
}
