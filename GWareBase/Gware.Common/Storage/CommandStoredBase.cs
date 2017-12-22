using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public abstract class CommandStoredBase : LoadedFromAdapterBase
    {
        public abstract IDataCommand CreateSaveCommand();
        public abstract IDataCommand CreateDeleteCommand();

        public abstract IDataCommand CreateLoadFromPrimaryKey(long primaryKey);
    }
}
