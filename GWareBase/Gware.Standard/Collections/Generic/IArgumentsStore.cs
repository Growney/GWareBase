using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public interface IArgumentsStore<T>
    {
        void DiscardArguments(string guid);
        T ReCallArguments(string guid);
        string StoreArguments(T parameters);
    }
}
