using Gware.Business.Commands;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity
{
    public class AllowedEntityAssignment : LoadedFromAdapterBase
    {
        private int m_id;
        private DateTime m_lastUpdate;
        private int m_fromEntityType;
        private int m_toEntityType;

        public int Id
        {
            get
            {
                return m_id;
            }

            set
            {
                m_id = value;
            }
        }

        public DateTime LastUpdate
        {
            get
            {
                return m_lastUpdate;
            }

            set
            {
                m_lastUpdate = value;
            }
        }

        public int FromEntityType
        {
            get
            {
                return m_fromEntityType;
            }

            set
            {
                m_fromEntityType = value;
            }
        }

        public int ToEntityType
        {
            get
            {
                return m_toEntityType;
            }

            set
            {
                m_toEntityType = value;
            }
        }


        public AllowedEntityAssignment()
        {

        }
        protected override void LoadFrom(IDataAdapter adapter)
        {
            m_id = adapter.GetValue("Id", 0);
            m_lastUpdate = adapter.GetValue("LastUpdate", SqlDateTime.MinValue.Value);
            m_fromEntityType = adapter.GetValue("ParentEntityType", -1);
            m_toEntityType = adapter.GetValue("ChildEntityType", -1);
        }

        public static List<AllowedEntityAssignment> LoadAllowedForEntityType(int entityType)
        {
            return Load<AllowedEntityAssignment>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadAllowedForEntityType(entityType)));
        }
    }
}
