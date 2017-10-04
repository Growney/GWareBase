using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity.Attributes
{
    public class AdapterLoadedFieldAttribute : Attribute
    {
        public string Field { get; private set; }
    }
}
