using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public class MSSQLStoredObjectCommandController : MSSQLCommandController, ICommandController
    {

        public MSSQLStoredObjectCommandController(string serverName, string databaseName, string databaseUsername, string databasePassword)
            : base(serverName, databaseName, databaseUsername, databasePassword)
        {

        }
    }
}
