using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity
{
    public class EntityTypeAttribute : Attribute
    {
        private int m_entityType;
        public int EntityType
        {
            get
            {
                return m_entityType;
            }

            set
            {
                m_entityType = value;
            }
        }
        public EntityTypeAttribute(int entityType)
        {
            m_entityType = entityType;
        }


    }
}
