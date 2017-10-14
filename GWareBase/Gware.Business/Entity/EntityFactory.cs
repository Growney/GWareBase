using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Gware.Common.DataStructures;
using Gware.Common.Storage;
using Gware.Business.Entity.Attributes;

namespace Gware.Business.Entity
{
    public class EntityFactory
    {
        private static object m_typeLock = new object();
        private static Dictionary<int, Type> m_entityTypes = new Dictionary<int, Type>();

        public static void InitialiseEntityTypes()
        {
            m_entityTypes.Clear();

            Assembly[] loaded = AppDomain.CurrentDomain.GetAssemblies();
            Task[] loadTasks = new Task[loaded.Length];

            for (int i = 0; i < loaded.Length; i++)
            {
                loadTasks[i] = CreateSearchTask(loaded[i]);
            }

            Task.WaitAll(loadTasks);
        }

        private static Task CreateSearchTask(Assembly assembly)
        {
            Assembly taskAssembly = assembly;
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    Type[] assemblyTypes = taskAssembly.GetTypes();
                    for(int i = 0; i < assemblyTypes.Length;i++)
                    {
                        if (assemblyTypes[i] != null)
                        {
                            Type type = assemblyTypes[i];
                            object[] attributes = type.GetCustomAttributes(typeof(EntityTypeAttribute), true);
                            if (attributes.Length > 0)
                            {
                                for (int j = 0; j < attributes.Length; j++)
                                {
                                    if (attributes[j] is EntityTypeAttribute)
                                    {
                                        EntityTypeAttribute attribute = attributes[j] as EntityTypeAttribute;
                                        if (attribute != null)
                                        {
                                            lock (m_typeLock)
                                            {
                                                m_entityTypes.Set(attribute.EntityType, type);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception)
                {

                }
                finally
                {

                }
                
            });
        }

        public static EntityBase CreateEntity(int entityTypeID)
        {
            lock (m_typeLock)
            {
                if(m_entityTypes.Count == 0)
                {
                    InitialiseEntityTypes();
                }
            }
            EntityBase retVal = null;
            if (m_entityTypes.ContainsKey(entityTypeID))
            {
                Type createType = m_entityTypes[entityTypeID];
                retVal = Activator.CreateInstance(createType) as EntityBase;
            }
            
            return retVal;
        }
    }
}
