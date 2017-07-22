using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public interface IStoredObjectCommandController : ICommandController
    {
        T ExecuteSingleCommand<T>(IDataCommand command) where T : StoredObjectBase, new();
        IList<T> ExecuteMultipleCommand<T>(IDataCommand command) where T : StoredObjectBase, new();
    }
}
