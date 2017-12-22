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

        public static DataCommand LoadEntity(int entityTypeID, long entityID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadEntity");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int64).Value = entityID;
            return retVal;
        }

        public static DataCommand LoadEntities(int entityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadEntities");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            return retVal;
        }

        public static DataCommand LoadChildEntity(long parentEntityTypeID, long parentEntityID, int childEntityTypeID, int index)
        {
            DataCommand retVal = Factory.CreateCommand("LoadChildEntity");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = parentEntityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = parentEntityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = childEntityTypeID;
            retVal.AddParameter("Index", DbType.Int32).Value = index;
            return retVal;
        }
        public static DataCommand LoadParentEntity(long childEntityTypeID, long childEntityID, int parentEntityTypeID, int index)
        {
            DataCommand retVal = Factory.CreateCommand("LoadParentEntity");
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = childEntityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = childEntityID;
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = parentEntityTypeID;
            retVal.AddParameter("Index", DbType.Int32).Value = index;
            return retVal;
        }
       

        public static DataCommand LoadParentEntites(long entityID, int entityTypeID, int parentEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadParentEntites");

            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            retVal.AddParameter("ParentEntityTypeID", DbType.Int32).Value = parentEntityTypeID;
            return retVal;
        }

        public static DataCommand LoadChildEntites(long entityID, int entityTypeID, int childEntityTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadChildEntities");

            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            retVal.AddParameter("ChildEntityTypeID", DbType.Int32).Value = childEntityTypeID;
            return retVal;
        }

        #endregion ---- Read ----

        #region ---- Write ----

        public static DataCommand Delete(int entityTypeID, long entityID)
        {
            DataCommand retVal = Factory.CreateCommand("Delete",true,false,
                LoadEntities(entityTypeID),
                LoadEntity(entityTypeID, entityID));
            retVal.AddParameter("EntityTypeID", DbType.Int32).Value = entityTypeID;
            retVal.AddParameter("EntityID", DbType.Int32).Value = entityID;
            return retVal;
        }


        
        #endregion ---- Write ----

    }
}
