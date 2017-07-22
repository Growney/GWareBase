using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Cache
{
    public delegate void RecacheTriggerEventHandler(object sender,IDataCommand command);
    public interface IRecacheTrigger
    {
        event RecacheTriggerEventHandler OnRecacheTrigger;
        event EventHandler OnClearCacheTrigger;
    }
}
