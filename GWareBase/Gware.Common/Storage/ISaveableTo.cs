using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    interface ISaveableTo<T>
    {
        void SaveTo(T to);
    }
}
