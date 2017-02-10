using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public interface IDataAdapter
        : 
        IDataTypeGetter<sbyte>,
        IDataTypeGetter<short>,
        IDataTypeGetter<int>,
        IDataTypeGetter<long>,
        IDataTypeGetter<decimal>,
        IDataTypeGetter<float>,
        IDataTypeGetter<double>,
        IDataTypeGetter<byte>,
        IDataTypeGetter<ushort>,
        IDataTypeGetter<uint>,
        IDataTypeGetter<ulong>,
        IDataTypeGetter<char>,
        IDataTypeGetter<string>,
        IDataTypeGetter<bool>,
        IDataTypeGetter<DateTime>
    {
        



    }
}
