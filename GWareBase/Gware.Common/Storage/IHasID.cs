using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage
{
    public interface IHasID
    {
        long Id
        {
            get;
        }
    }
}
