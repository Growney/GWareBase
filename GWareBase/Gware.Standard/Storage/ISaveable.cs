using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Storage
{
    public interface ISaveable
    {
        long Save(ICommandController controller);
        Task<long> SaveAsync(ICommandController controller);
    }
}
