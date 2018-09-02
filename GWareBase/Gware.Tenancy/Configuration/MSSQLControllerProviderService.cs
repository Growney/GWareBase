using Gware.Common.Context;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Configuration
{
    public class MSSQLControllerProvider : IControllerProvider
    {
        private readonly IConfiguration m_configuration;

        public MSSQLControllerProvider(IConfiguration config)
        {
            m_configuration = config;
        }

        public ICommandController CreateController(string key)
        {
            return m_configuration.CreateController(key);
        }
    }

    public static class MSSQLControllerProviderExtensions
    {
        public static ICommandController CreateController(this IConfiguration configuration,string key)
        {
            bool isTrusted = configuration[$"Controllers:{key}:Trusted"]?.ToLower().ToString() == "true";
            if (isTrusted)
            {
                return new MSSQLCommandController(configuration[$"Controllers:{key}:Server"], configuration[$"Controllers:{key}:Databasename"]);
            }
            else
            {
                return new MSSQLCommandController(configuration[$"Controllers:{key}:Server"], configuration[$"Controllers:{key}:Databasename"], configuration[$"Controllers:{key}:Username"], configuration[$"Controllers:{key}:Password"]);
            }
        }
    }
}

