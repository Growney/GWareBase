using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Delegates
{
    public delegate void AsyncListReturn<T>(object sender,List<T> returnValue);
}
