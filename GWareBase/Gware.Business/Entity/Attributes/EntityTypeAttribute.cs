using Gware.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity.Attributes
{
    public class EntityTypeAttribute : ClassIDAttribute
    {
        public EntityTypeAttribute(int classID) : base(classID)
        {
        }
    }
}
