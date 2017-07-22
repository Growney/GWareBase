using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gware.Common.DataStructures;

namespace Gware.Business.Entity.Collection
{
    internal class StoredMultiEntityTypeCollection : IMultiEntityTypeStorage
    {
        private EntityRelationship m_relationship;
        private EntityBase m_relation;
        private Dictionary<int, List<EntityBase>> m_collection;

        public EntityRelationship Relationship
        {
            get
            {
                return m_relationship;
            }
            set
            {
                m_relationship = value;
            }
        }

        public EntityBase Relation
        {
            get
            {
                return m_relation;
            }
            set
            {
                m_relation = value;
            }
        }

        
        public StoredMultiEntityTypeCollection(EntityBase relation, EntityRelationship relationship)
        {
            m_relation = relation;
            m_relationship = relationship;
            m_collection = new Dictionary<int, List<EntityBase>>();
        }

        private List<EntityBase> GetCollection(int entityType)
        {
            if (!m_collection.ContainsKey(entityType))
            {
                m_collection.Add(entityType, new List<EntityBase>());
            }
            return m_collection[entityType];
        }


        private void SetCollection(int entityTypeID,List<EntityBase> collection)
        {
            m_collection.Set(entityTypeID, collection);
        }
        public void Add<T>(IEnumerable<T> items) where T : EntityBase
        {
            foreach(T item in items)
            {
                Add(item);
            }
        }

        public void Add<T>(T item) where T : EntityBase
        {
            GetCollection(item.EntityTypeID).Add(item);
            switch (m_relationship)
            {
                case EntityRelationship.Parent:
                    EntityAssignment.Save(m_relation.GetID(), m_relation.EntityTypeID, item.GetID(), item.EntityTypeID, -1);
                    break;
                case EntityRelationship.Child:
                    EntityAssignment.Save(item.GetID(), item.EntityTypeID, m_relation.GetID(), m_relation.EntityTypeID, -1);
                    break;
            }
        }

        public bool Exists<T>(int entityTypeID, int index) where T : EntityBase, new()
        {
            T retVal = default(T);
            switch (m_relationship)
            {
                case EntityRelationship.Parent:
                    retVal = EntityBase.LoadChildEntity<T>(m_relation.EntityTypeID, m_relation.GetID(), entityTypeID, index);
                    break;
                case EntityRelationship.Child:
                    retVal = EntityBase.LoadParentEntity<T>(m_relation.EntityTypeID, m_relation.GetID(), entityTypeID, index);
                    break;
            }

            return retVal != default(T);
        }

        public T Get<T>(int entityTypeID, int index) where T : EntityBase,new()
        {
            T retVal = default(T);
            switch (m_relationship)
            {
                case EntityRelationship.Parent:
                    retVal = EntityBase.LoadChildEntity<T>(m_relation.EntityTypeID, m_relation.GetID(), entityTypeID, index);
                    break;
                case EntityRelationship.Child:
                    retVal = EntityBase.LoadParentEntity<T>(m_relation.EntityTypeID, m_relation.GetID(), entityTypeID, index);
                    break;
            }

            if(index < 0)
            {
                GetCollection(entityTypeID).Add(retVal);
            }
            else
            {
                GetCollection(entityTypeID).Insert(index, retVal);
            }

            return retVal;
        }

        public List<T> Get<T>(int entityTypeID) where T : EntityBase,new()
        {
            List<T> retVal = new List<T>();

            switch (m_relationship)
            {
                case EntityRelationship.Parent:
                    retVal.AddRange(EntityBase.LoadChildEntities<T>(m_relation.GetID(), m_relation.EntityTypeID, entityTypeID));
                    break;
                case EntityRelationship.Child:
                    retVal.AddRange(EntityBase.LoadParentEntities<T>(m_relation.GetID(), m_relation.EntityTypeID, entityTypeID));
                    break;
            }

            GetCollection(entityTypeID).Clear();
            GetCollection(entityTypeID).AddRange(retVal);

            return retVal;
        }

        public void Set<T>(int entityTypeID, int index, T item) where T : EntityBase
        {
            switch (m_relationship)
            {
                case EntityRelationship.Parent:
                    EntityAssignment.Save(m_relation.GetID(), m_relation.EntityTypeID, item.GetID(), entityTypeID, index);
                    break;
                case EntityRelationship.Child:
                    EntityAssignment.Save(item.GetID(), entityTypeID, m_relation.GetID(), m_relation.EntityTypeID, index);
                    break;
            }

            GetCollection(entityTypeID).Insert(index, item);
        }

        public IEnumerator<EntityBase> GetEnumerator()
        {
            foreach(int key in m_collection.Keys)
            {
                foreach(EntityBase entity in m_collection[key])
                {
                    yield return entity;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
