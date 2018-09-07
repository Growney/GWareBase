using Gware.Standard.Storage.Adapter;
using Gware.Standard.Storage.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Storage.Controller
{
    public interface ICommandController
    {
        IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command);
        IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command);
        int ExecuteQuery(IDataCommand command);

        Task<IDataAdapterCollectionGroup> ExecuteGroupCommandAsync(IDataCommand command);
        Task<IDataAdapterCollection> ExecuteCollectionCommandAsync(IDataCommand command);
        Task<int> ExecuteQueryAsync(IDataCommand command);

        string GetInitialisationString();
        void Initialise(string initialisationString);
        ICommandController Clone();
        void SetName(string name);
    }
}
