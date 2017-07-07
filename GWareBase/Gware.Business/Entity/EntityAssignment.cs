using Gware.Business.Commands;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity
{
    public class EntityAssignment : StoredObjectBase
    {

        private DateTime m_lastupdate;
        private int m_parentEntityID;
        private int m_parentEntityTypeID;
        private int m_childEntityID;
        private int m_childEntityTypeID;


        public DateTime Lastupdate
        {
            get
            {
                return m_lastupdate;
            }

            set
            {
                m_lastupdate = value;
            }
        }
        public int ParentEntityID
        {
            get
            {
                return m_parentEntityID;
            }

            set
            {
                m_parentEntityID = value;
            }
        }
        public int ParentEntityTypeID
        {
            get
            {
                return m_parentEntityTypeID;
            }

            set
            {
                m_parentEntityTypeID = value;
            }
        }
        public int ChildEntityID
        {
            get
            {
                return m_childEntityID;
            }

            set
            {
                m_childEntityID = value;
            }
        }
        public int ChildEntityTypeID
        {
            get
            {
                return m_childEntityTypeID;
            }

            set
            {
                m_childEntityTypeID = value;
            }
        }

        public EntityAssignment()
        {

        }
        protected override void OnLoad(IDataAdapter adapter)
        {
            m_lastupdate = adapter.GetValue("LastUpdate", SqlDateTime.MinValue.Value);
            m_parentEntityID = adapter.GetValue("ParentEntityID", 0);
            m_parentEntityTypeID = adapter.GetValue("ParentEntityTypeID", 0);
            m_childEntityID = adapter.GetValue("ChildEntityTypeID", 0);
            m_childEntityTypeID = adapter.GetValue("ChildEntityTypeID", 0);

        }

        public override IDataCommand CreateSaveCommand()
        {
            return EntityCommandFactory.SaveEntityAssignment(m_parentEntityID, m_parentEntityTypeID, m_childEntityID, m_childEntityTypeID);
        }

        public override IDataCommand CreateDeleteCommand()
        {
            return EntityCommandFactory.SaveEntityAssignment(m_parentEntityID, m_parentEntityTypeID, m_childEntityID, m_childEntityTypeID);
        }

        public static int Save(int fromEntityID, int fromEntityTypeID, int toEntityID, int toEntityTypeID)
        {
            EntityAssignment val = new EntityAssignment()
            {
                ParentEntityID = fromEntityID,
                ParentEntityTypeID = fromEntityTypeID,
                ChildEntityID = toEntityID,
                ChildEntityTypeID = toEntityTypeID
            };
            return val.Save();
        }
        public static bool Delete(int fromEntityID, int fromEntityTypeID, int toEntityID, int toEntityTypeID)
        {
            EntityAssignment val = new EntityAssignment()
            {
                ParentEntityID = fromEntityID,
                ParentEntityTypeID = fromEntityTypeID,
                ChildEntityID = toEntityID,
                ChildEntityTypeID = toEntityTypeID
            };
            return val.Delete();
        }
        public static bool Delete(int fromEntityID, int fromEntityTypeID)
        {
            return LoadSingle<LoadedFromAdapterValue<bool>>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.DeleteEntityAssignment(fromEntityID, fromEntityTypeID))).Value;
        }

    }
}
