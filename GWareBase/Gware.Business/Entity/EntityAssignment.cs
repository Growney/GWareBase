using Gware.Business.Commands;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
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
        private long m_parentEntityID;
        private int m_parentEntityTypeID;
        private long m_childEntityID;
        private int m_childEntityTypeID;
        private int m_index;
        private long m_id;

        public override long Id
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
        public long ParentEntityID
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
        public long ChildEntityID
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

        public int Index
        {
            get
            {
                return m_index;
            }

            set
            {
                m_index = value;
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
            m_index = adapter.GetValue("Index", 0);

        }

        public override IDataCommand CreateSaveCommand()
        {
            return EntityAssignmentCommandFactory.SaveEntityAssignment(m_parentEntityID, m_parentEntityTypeID, m_childEntityID, m_childEntityTypeID, m_index);
        }

        public override IDataCommand CreateDeleteCommand()
        {
            return EntityAssignmentCommandFactory.DeleteEntityAssignment(m_parentEntityID, m_parentEntityTypeID, m_childEntityID, m_childEntityTypeID, m_index);
        }

        public static long Save(ICommandController controller,long fromEntityID, int fromEntityTypeID, long toEntityID, int toEntityTypeID, int index)
        {
            EntityAssignment val = new EntityAssignment()
            {
                ParentEntityID = fromEntityID,
                ParentEntityTypeID = fromEntityTypeID,
                ChildEntityID = toEntityID,
                ChildEntityTypeID = toEntityTypeID,
                Index = index
            };
            return val.Save(controller);
        }
        public static void Delete(ICommandController controller,int fromEntityID, int fromEntityTypeID, int toEntityID, int toEntityTypeID, int index)
        {
            EntityAssignment val = new EntityAssignment()
            {
                ParentEntityID = fromEntityID,
                ParentEntityTypeID = fromEntityTypeID,
                ChildEntityID = toEntityID,
                ChildEntityTypeID = toEntityTypeID,
                Index = index
            };
            val.Delete(controller);
        }

        public override IDataCommand CreateLoadFromPrimaryKey(long primaryKey)
        {
            return null;
        }
    }
}
