using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Cache
{
    public class ManualRecacheTrigger : IRecacheTrigger
    {
        public event RecacheTriggerEventHandler OnRecacheTrigger;
        public event EventHandler OnClearCacheTrigger;

        public void TriggerRecache(IDataCommand val)
        {
            OnRecacheTrigger?.Invoke(this, val);
        }

        public void TriggerClearCache()
        {
            OnClearCacheTrigger?.Invoke(this, new EventArgs());
        }
    }
}
