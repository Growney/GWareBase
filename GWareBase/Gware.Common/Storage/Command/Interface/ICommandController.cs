using Gware.Common.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Interface
{
    public interface ICommandController
    {
        IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command);
        IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command);
        int ExecuteQuery(IDataCommand command);

        string GetInitialisationString();
        void Initialise(string initialisationString);
        ICommandController Clone();
        void SetName(string name);
    }
}
