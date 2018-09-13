using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gware.Standard.Storage.Controller
{
    public static class CommandControllerFactory
    {
        public static string GetCreationString(ICommandController controller)
        {
            return $"{controller.GetType().FullName}?{controller.GetInitialisationString()}";
        }
        public static ICommandController CreateController(Assembly[] searchIn,string  initString)
        {
            ICommandController retVal = null;
            string[] split = initString.Split('?');

            if (split.Length > 1)
            {
                Type controllerType = null;
                for (int i = 0; i < searchIn.Length; i++)
                {
                    Assembly assembly = searchIn[i];
                    controllerType = assembly.GetType(split[0]);
                    if(controllerType != null)
                    {
                        break;
                    }
                }
                if(controllerType != null)
                {
                    retVal = Activator.CreateInstance(controllerType) as ICommandController;
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
