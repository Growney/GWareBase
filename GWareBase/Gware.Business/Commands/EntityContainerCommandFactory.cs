using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Commands
{
    public class EntityContainerCommandFactory: DataCommandFactory
    {

        private static object m_factoryCreationLock = new object();
        private static EntityContainerCommandFactory m_factory;

        public static EntityContainerCommandFactory Factory
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
                            m_factory = new EntityContainerCommandFactory();
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

        public EntityContainerCommandFactory() : base("EntityContainer", true)
        {

        }
        public static DataCommand Save(int id, int entityContainerTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("Save",
                Load(entityContainerTypeID),
                LoadSingle(id,entityContainerTypeID));
            retVal.AddParameter("Id", DbType.Int32).Value = id;
            retVal.AddParameter("EntityContainerTypeID", DbType.String).Value = entityContainerTypeID;
            return retVal;
        }

        public static DataCommand LoadSingle(int id, int entityContainerTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("LoadSingle");
            retVal.AddParameter("Id", DbType.Int32).Value = id;
            retVal.AddParameter("EntityContainerTypeID", DbType.String).Value = entityContainerTypeID;
            return retVal;
        }

        public static DataCommand Load(int entityContainerTypeID)
        {
            DataCommand retVal = Factory.CreateCommand("Load");
            retVal.AddParameter("EntityContainerTypeID", DbType.String).Value = entityContainerTypeID;
            return retVal;
        }
    }
}
