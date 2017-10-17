using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Reflection
{
    public class ClassIDAttribute : Attribute
    {
        public int ClassID { get;set; }

        public ClassIDAttribute(int classID)
        {
            ClassID = classID;
        }
    }
}
