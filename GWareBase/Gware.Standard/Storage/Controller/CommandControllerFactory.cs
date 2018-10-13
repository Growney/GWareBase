using Microsoft.Extensions.DependencyInjection;
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
            return $"{controller.GetInitialisationString()}";
        }
        
        public static ICommandController CreateController<T>(IServiceProvider provider, string initString) where T : ICommandController
        {
            ICommandController controller = ActivatorUtilities.CreateInstance<T>(provider);
            controller.Initialise(initString);
            return controller;
        }
    }
}
