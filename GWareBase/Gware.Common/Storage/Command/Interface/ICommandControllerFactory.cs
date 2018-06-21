using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Interface
{
    public interface ICommandControllerFactory
    {
        bool TryParse(string val, out ICommandController controller);
        string GenerateString(ICommandController controller);
    }
}
