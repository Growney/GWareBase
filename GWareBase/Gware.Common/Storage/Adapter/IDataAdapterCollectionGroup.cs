using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public interface IDataAdapterCollectionGroup
    {
        IDataAdapterCollection[] Collections { get; }
    }
}
