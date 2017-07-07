using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Business.Entity;
using Gware.Common.DataStructures;
namespace Gware.Business.Entity.Collection
{
    internal enum EntityRelationship
    {
        Parent,
        Child
    }
    internal class MultiEntityTypeCollection : IEnumerable<EntityBase>
    {
        private EntityRelationship m_relationship;
        private EntityBase m_relation;
        private Dictionary<int, List<EntityBase>> m_internalCollection = new Dictionary<int, List<EntityBase>>();

        public MultiEntityTypeCollection(EntityBase relation,EntityRelationship relationship)
        {
            m_relation = relation;
            m_relationship = relationship;
        }
        
        public void Add<T>(List<T> items) where T : EntityBase
        {
            for (int i = 0; i < items.Count; i++)
            {
                Add(items[i]);
            }
        }

        public void Add<T>(T item) where T : EntityBase
        {
            if(item != null)
            {
                int entityTypeID = typeof(T).GetEntityTypeID();
                m_internalCollection.AddItem<int,EntityBase,List<EntityBase>>(entityTypeID, item);
            }
        }

        public void Set<T>(int index,int entityTypeID,T item)
        {
            m_internalCollection.Set(index, entityTypeID, item);
        }

        public T Get<T>(int index,int entityTypeID) where T : EntityBase
        {
            T retVal = default(T);

            if (m_internalCollection.ContainsKey(entityTypeID))
            {
                retVal = (T)m_internalCollection[entityTypeID].Get(index);
            }

            return retVal;
        }
        public IReadOnlyList<T> Get<T>(int entityTypeID) where T : EntityBase
        {
            List<T> retVal = new List<T>();

            if (m_internalCollection.ContainsKey(entityTypeID))
            {
                List<EntityBase> storedCollection = m_internalCollection[entityTypeID];
                for (int i = 0; i < storedCollection.Count; i++)
                {
                    if(storedCollection[i] != null)
                    {
                        retVal.Add((T)storedCollection[i]);
                    }
                }
            }

            return retVal.AsReadOnly();
        }
        public IEnumerator<EntityBase> GetEnumerator()
        {
            foreach(int entityTypeID in m_internalCollection.Keys)
            {
                foreach(EntityBase entity in m_internalCollection[entityTypeID])
                {
                    if(entity != null)
                    {
                        yield return entity;
                    }
                }
            }
        }

        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        private T LoadAssignedSingle<T>(int index, int entityTypeID)
        {
            T retVal = default(T);

            switch (m_relationship)
            {
                case EntityRelationship.Parent:

                    break;
                case EntityRelationship.Child:
                    break;
                default:
                    break;
            }

            return retVal;
        }

    }
}
