using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Application
{
    public interface IArgumentsStore<T>
    {
        T ReCallArguments(string guid);
        string StoreArguments(T parameters);
        void DiscardArguments(string guid);
    }
}
