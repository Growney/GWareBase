using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Commands
{
    public class EntityAssignmentCommandFactory : DataCommandFactory
    {
        private static object m_factoryCreationLock = new object();
        private static EntityAssignmentCommandFactory m_factory;

        public static EntityAssignmentCommandFactory Factory
        {
            get
            {

                if (m_factory == null)
                {
                    // Double check to allow the first check to be performed without locking the object and the second to ensure that nothing has aquired the lock after the check was performed
                    lock (m_factoryCreationLock)
                    {
                        if (m_factory == null)
                        {
                            m_factory = new EntityAssignmentCommandFactory();
                        }
                    }

                }
                return m_factory;
            }
            set
            {
                m_factory = value;
            }
        }

        protected EntityAssignmentCommandFactory(string commandName, bool requiredAuthentication, bool cache) : base(commandName, requiredAuthentication, cache)
        {

        }
        protected EntityAssignmentCommandFactory(string commandName, bool requiredAuthentication) : base(commandName, requiredAuthentication)
        {

        }
        protected EntityAssignmentCommandFactory(string commandName) : this(commandName, true)
        {

        }

        public EntityAssignmentCommandFactory() : base("EntityAssignment", true, false)
        {

        }

        public static DataCommand SaveEntityAssignment(long fromEntityID, int fromEntityTypeID, long toEntityID, int toEntityTypeID, int index)
        {
            DataCommand retVal = Factory.CreateCommand("SaveEntityAssignment", GetWriteAssignmentRecache(fromEntityID, fromEntityTypeID, toEntityID, toEntityTypeID, index));
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = fromEntityTypeID;
            retVal.AddParameter("ParentEntityID", DbType.Int32).Value = fromEntityID;
            retVal.AddParameter("ChildEntityID", DbType.Int32).Value = toEntityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = toEntityTypeID;
            retVal.AddParameter("Index", DbType.Int32).Value = index;
            return retVal;
        }
        public static DataCommand DeleteEntityAssignment(long fromEntityID, int fromEntityTypeID, long toEntityID, int toEntityTypeID, int index)
        {
            DataCommand retVal = Factory.CreateCommand("DeleteEntityAssignment", GetWriteAssignmentRecache(fromEntityID, fromEntityTypeID, toEntityID, toEntityTypeID, index));
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = fromEntityTypeID;
            retVal.AddParameter("ParentEntityID", DbType.Int32).Value = fromEntityID;
            retVal.AddParameter("ChildEntityID", DbType.Int32).Value = toEntityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = toEntityTypeID;
            retVal.AddParameter("Index", DbType.Int32).Value = index;
            return retVal;
        }

        private static IDataCommand[] GetWriteAssignmentRecache(long fromEntityID, int fromEntityTypeID, long toEntityID, int toEntityTypeID, int index)
        {
            return new IDataCommand[] {
                EntityCommandFactory.LoadChildEntity(fromEntityID, fromEntityTypeID,toEntityTypeID,index),
                EntityCommandFactory.LoadParentEntity(toEntityID, toEntityTypeID,fromEntityTypeID, index)
            };
        }
    }
}
