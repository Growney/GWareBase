using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public class MSSQLStoredObjectCommandController : MSSQLCommandController, IStoredObjectCommandController
    {

        public MSSQLStoredObjectCommandController(string serverName, string databaseName, string databaseUsername, string databasePassword)
            : base(serverName, databaseName, databaseUsername, databasePassword)
        {

        }

        public IList<T> ExecuteMultipleCommand<T>(IDataCommand command) where T : StoredObjectBase, new()
        {
            return LoadedFromAdapterBase.Load<T>(ExecuteCollectionCommand(command));
        }

        public T ExecuteSingleCommand<T>(IDataCommand command) where T : StoredObjectBase, new()
        {
            return LoadedFromAdapterBase.LoadSingle<T>(ExecuteCollectionCommand(command));
        }
    }
}
