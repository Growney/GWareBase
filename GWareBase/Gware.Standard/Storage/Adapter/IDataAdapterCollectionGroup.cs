using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Adapter
{
    public interface IDataAdapterCollectionGroup
    {
        IDataAdapterCollection[] Collections { get; }
    }
}
