using Gware.Business.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity
{
    public static class ExtensionMethods
    {
        public static int GetEntityTypeID(this Type type)
        {
            int retVal = -1;

            object[] attributes = type.GetCustomAttributes(false);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is EntityTypeAttribute)
                {
                    retVal = (int)(attributes[i] as EntityTypeAttribute).EntityType;
                    break;
                }
            }
            return retVal;
        }
    }
}
