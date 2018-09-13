using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Common.Storage.Command
{
    public static class CommandControllerFactory
    {
        public static string GetCreationString(ICommandController controller)
        {
            Type type = controller.GetType();
            return $"{type.Assembly.FullName}#{type.Name}?{controller.GetInitialisationString()}";
        }
        public static ICommandController CreateController(string initString)
        {
            ICommandController retVal = null;
            string[] split = initString.Split('?');

            if(split.Length > 1)
            {
                string[] typeSplit = split[0].Split('#');
                if(typeSplit.Length > 1)
                {
                    retVal = Activator.CreateInstance(typeSplit[0],typeSplit[1]) as ICommandController;
                    if (retVal != null)
                    {
                        retVal.Initialise(split[1]);
                    }
                }
                
            }

            return retVal;
            
        }
    }
}
