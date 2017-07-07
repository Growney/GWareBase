using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gware.Business.Commands
{
    public class EntityCommandFactory : DataCommandFactory
    {
        private static object m_factoryCreationLock = new object();
        private static EntityCommandFactory m_factory;

        public static EntityCommandFactory Factory
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
                            m_factory = new EntityCommandFactory();
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
        protected EntityCommandFactory(string commandName, bool requiredAuthentication, bool cache) : base(commandName, requiredAuthentication, cache)
        {

        }
        protected EntityCommandFactory(string commandName, bool requiredAuthentication) : base(commandName, requiredAuthentication)
        {

        }
        protected EntityCommandFactory(string commandName) : this(commandName, true)
        {

        }

        public EntityCommandFactory() : base("Entity", true, true)
        {

        }

        #region ---- Read ----

        public static DataCommand LoadEntity(int entityTypeID, int entityID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadEntity");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            return retVal;
        }
        public static DataCommand LoadChildEntity(int parentEntityTypeID, int parentEntityID, int childEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadChildEntity");
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = parentEntityTypeID;
            retVal.AddParameter("ParentEntityID", DbType.Int32).Value = parentEntityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = childEntityTypeID;
            return retVal;
        }
        public static DataCommand LoadEntities(int entityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadEntities");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            return retVal;
        }
        public static DataCommand LoadAllowedForEntityType(int entityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadAllowedForEntityType");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            return retVal;
        }

        public static DataCommand LoadChildEntities(int entityTypeID, int entityID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadChildEntites");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            return retVal;
        }

        public static DataCommand LoadParentEntities(int entityTypeID, int entityID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadParentEntities");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            return retVal;
        }

        public static DataCommand LoadChildEntitiesWithType(int entityID, int entityTypeID, int childEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadChildEntitiesWithType");

            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = childEntityTypeID;
            return retVal;
        }

        public static DataCommand LoadParentEntitiesWithType(int entityID, int entityTypeID, int parentEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadParentEntitiesWithType");

            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = parentEntityTypeID;
            return retVal;
        }

        #endregion ---- Read ----

        #region ---- Write ----

        public static DataCommand Delete(int entityTypeID, int entityID)
        {
            DataCommand retVal = Factory.CreateCommand("Delete",
                LoadEntities(entityTypeID),
                LoadEntity(entityTypeID, entityID));
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            return retVal;
        }


        public static DataCommand SaveEntityAssignment(int fromEntityID, int fromEntityTypeID, int toEntityID, int toEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("SaveEntityAssignment",
                LoadParentEntities(fromEntityID, fromEntityTypeID),
                LoadChildEntities(fromEntityID, toEntityTypeID));
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = fromEntityTypeID;
            retVal.AddParameter("ParentEntityID", DbType.Int32).Value = fromEntityID;
            retVal.AddParameter("ChildEntityID", DbType.Int32).Value = toEntityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = toEntityTypeID;
            return retVal;
        }
        public static DataCommand DeleteEntityAssignment(int fromEntityID, int fromEntityTypeID, int toEntityID, int toEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("DeleteEntityAssignment",
                LoadParentEntities(fromEntityID, fromEntityTypeID),
                LoadChildEntities(fromEntityID, toEntityTypeID));
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = fromEntityTypeID;
            retVal.AddParameter("ParentEntityID", DbType.Int32).Value = fromEntityID;
            retVal.AddParameter("ChildEntityID", DbType.Int32).Value = toEntityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = toEntityTypeID;
            return retVal;
        }
        public static DataCommand DeleteEntityAssignment(int fromEntityID, int fromEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("DeleteAllEntityAssignment",
                LoadParentEntities(fromEntityID, fromEntityTypeID));
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = fromEntityTypeID;
            retVal.AddParameter("ParentEntityID", DbType.Int32).Value = fromEntityID;
            return retVal;
        }

        #endregion ---- Write ----

    }
}
