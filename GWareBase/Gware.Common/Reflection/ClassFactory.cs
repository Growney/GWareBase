using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.DataStructures;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;

namespace Gware.Common.Reflection
{
    public static class ClassFactory<AttributeType,ClassType> where AttributeType : ClassIDAttribute
    {
        private static object m_typeLock = new object();
        private static Dictionary<int, Type> m_classCache = new Dictionary<int, Type>();

        public static void InitialiseEntityTypes()
        {
            m_classCache.Clear();

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
                    for (int i = 0; i < assemblyTypes.Length; i++)
                    {
                        if (assemblyTypes[i] != null)
                        {
                            Type type = assemblyTypes[i];
                            object[] attributes = type.GetCustomAttributes(typeof(AttributeType), true);
                            if (attributes.Length > 0)
                            {
                                for (int j = 0; j < attributes.Length; j++)
                                {
                                    if (attributes[j] is AttributeType)
                                    {
                                        AttributeType attribute = attributes[j] as AttributeType;
                                        if (attribute != null)
                                        {
                                            lock (m_typeLock)
                                            {
                                                m_classCache.Set(attribute.ClassID, type);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {

                }

            });
        }
        public static ClassType CreateClass(int classID)
        {
            lock (m_typeLock)
            {
                if (m_classCache.Count == 0)
                {
                    InitialiseEntityTypes();
                }
            }
            ClassType retVal = default(ClassType);
            if (m_classCache.ContainsKey(classID))
            {
                Type createType = m_classCache[classID];
                retVal = (ClassType)Activator.CreateInstance(createType);
            }

            return retVal;
        }

        public static StoredClass CreateStoredClass<StoredClassAttribute,StoredClass>(string typeField,IDataAdapter adapter) where StoredClass : StoredObjectBase where StoredClassAttribute : ClassIDAttribute
        {
            StoredClass retVal = null;
            if(adapter != null)
            {
                int typeID = adapter.GetValue(typeField, 0);
                retVal = ClassFactory<StoredClassAttribute, StoredClass>.CreateClass(typeID);
                if (retVal != null)
                {
                    retVal.Load(adapter);
                }
            }
            
            return retVal;
        }

        public static IEnumerable<StoredClass> CreateStoredClass<StoredClassAttribute, StoredClass>(string typeField, IDataAdapterCollection adapters) where StoredClass : StoredObjectBase where StoredClassAttribute : ClassIDAttribute
        {
            List<StoredClass> retVal = new List<StoredClass>();

            foreach (IDataAdapter adapter in adapters.Adapters)
            {
                StoredClass newClass = ClassFactory<StoredClassAttribute, StoredClass>.CreateStoredClass<StoredClassAttribute, StoredClass>(typeField, adapter);
                if(newClass != null)
                {
                    retVal.Add(newClass);
                }
            }

            return retVal;
        }

    }
}
