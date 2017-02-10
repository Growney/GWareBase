using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API.Session
{
    public interface ISessonKey
    {
        string Value { get; }
        DateTime Expiry { get; }
        DateTime Issued { get; }
    }
}
