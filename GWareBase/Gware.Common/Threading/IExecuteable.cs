using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Threading
{
    public interface IExecuteable
    {
        void Execute();
        void Cancel();
        void Pause();
        void Resume();
    }
}
