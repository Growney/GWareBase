using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public abstract class GuidArgumentStore<T> : IArgumentsStore<T>
    {
        public ILogger<GuidArgumentStore<T>> Logger { get; }
        public GuidArgumentStore(ILogger<GuidArgumentStore<T>> logger)
        {
            Logger = logger;
        }
        public T ReCallArguments(string guid)
        {
            
            T retVal = default(T);
            try
            {
                if (Guid.TryParse(guid, out Guid newGuid))
                {
                    retVal = RecallArguments(newGuid);
                    if(retVal != default)
                    {
                        Logger.LogTrace($"Recalled GUID arguments {guid}");
                    }
                    else
                    {
                        Logger.LogWarning($"GUID arguments not found {guid}");
                    }
                }
                else
                {
                    Logger.LogWarning($"Failed to parse guid {guid}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error recalling guid arguments");
            }
            return retVal;
        }

        public abstract T RecallArguments(Guid guid);

        public string StoreArguments(T parameters)
        {
            Guid paramGuid = Guid.NewGuid();

            Logger.LogTrace($"Storing new arguments {paramGuid}");

            StoreArguments(paramGuid, parameters);

            return paramGuid.ToString();
        }

        public abstract void StoreArguments(Guid guid,T arguments);

        public void DiscardArguments(string guid)
        {
            if (Guid.TryParse(guid, out Guid newGuid))
            {
                if (DiscardArguments(newGuid))
                {
                    Logger.LogTrace($"Discraded arguments {newGuid}");
                }
                else
                {
                    Logger.LogWarning($"Could not find arguments to discard {newGuid}");
                }
            }
            else
            {
                Logger.LogWarning($"Failed to parse guid {guid}");
            }
        }

        public abstract bool DiscardArguments(Guid guid);
    }
}
