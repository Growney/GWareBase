using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Collections.Generic
{
    public abstract class GuidArgumentStore<T> : IArgumentsStore<T>
    {
        public T ReCallArguments(string guid)
        {
            T retVal = default(T);
            try
            {
                if (Guid.TryParse(guid, out Guid newGuid))
                {
                    retVal = RecallArguments(newGuid);
                }
            }
            catch (Exception)
            {
            }
            return retVal;
        }

        public abstract T RecallArguments(Guid guid);

        public string StoreArguments(T parameters)
        {
            Guid paramGuid = Guid.NewGuid();

            StoreArguments(paramGuid, parameters);

            return paramGuid.ToString();
        }

        public abstract void StoreArguments(Guid guid,T arguments);

        public void DiscardArguments(string guid)
        {
            if (Guid.TryParse(guid, out Guid newGuid))
            {
                DiscardArguments(newGuid);
            }
        }

        public abstract void DiscardArguments(Guid guid);
    }
}
