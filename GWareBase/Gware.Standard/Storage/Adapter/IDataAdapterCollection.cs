using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Adapter
{
    public interface IDataAdapterCollection
    {
        IDataAdapter[] Adapters { get; }
        IDataAdapter First { get; }
    }
}
