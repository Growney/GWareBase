using Gware.Business.Application;
using Gware.Business.Commands;
using Gware.Business.Entity.Attributes;
using Gware.Business.Entity.Collection;
using Gware.Common.Application;
using Gware.Common.Reflection;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;

namespace Gware.Business.Entity
{
    public abstract class EntityBase : StoredObjectBase
    {
        private DateTime m_lastUpdated;
        private readonly int m_entityTypeID;

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
        public int EntityTypeID
        {
            get
            {
                return m_entityTypeID;
            }
        }

        public DateTime LastUpdated
        {
            get
            {
                return m_lastUpdated;
            }

            set
            {
                m_lastUpdated = value;
            }
        }



        private IMultiEntityTypeStorage m_parents;
        private IMultiEntityTypeStorage m_children;

        public EntityBase()
        {
            m_entityTypeID = GetType().GetClassID<EntityTypeAttribute>();
            m_parents = new StoredMultiEntityTypeCollection(this, EntityRelationship.Child);
            m_children = new StoredMultiEntityTypeCollection(this, EntityRelationship.Parent);
        }

        internal long GetID(ICommandController controller)
        {
            if (Id == 0)
            {
                Save(controller);
            }

            return Id;
        }

        #region --- Set Parent ---

        public void SetParentEntity<T>(ICommandController controller,IConvertible entityTypeID, T item) where T : EntityBase, new()
        {
            SetParentEntity<T>(controller,entityTypeID, 0, item);
        }
        public void SetParentEntity<T>(ICommandController controller,IConvertible entityTypeID, int index, T item) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            m_parents.Set<T>(controller,entityTypeIDVal, index, item);
        }
        public void AddParentEntity<T>(ICommandController controller,IEnumerable<T> items) where T : EntityBase
        {
            m_parents.Add(controller,items);
        }
        public void AddParentEntity<T>(ICommandController controller,T item) where T : EntityBase
        {
            m_parents.Add(controller,item);
        }

        #endregion --- Set Parent ---

        #region --- Get Parent ---

        public bool ParentExists<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            return ParentExists<T>(controller,entityTypeID, -1);
        }

        public bool ParentExists<T>(ICommandController controller,IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_parents.Exists<T>(controller,entityTypeIDVal, index);
        }

        public T GetOneToOneParentEntity<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntity<T>(controller,entityTypeID, 0);
        }

        public T GetOneToManyParentEntity<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntity<T>(controller,entityTypeID, -1);
        }

        public T GetParentEntity<T>(ICommandController controller,IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_parents.Get<T>(controller,entityTypeIDVal, index);
        }
        public IReadOnlyList<T> GetParentEntites<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_parents.Get<T>(controller,entityTypeIDVal).AsReadOnly();
        }

        #endregion --- Get Parent ---

        #region --- Set Children ---
        public void SetChildEntity<T>(ICommandController controller,IConvertible entityTypeID, T item) where T : EntityBase, new()
        {
            SetChildEntity<T>(controller,entityTypeID, 0, item);
        }
        public void SetChildEntity<T>(ICommandController controller,IConvertible entityTypeID, int index, T item) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            m_children.Set<T>(controller,entityTypeIDVal, index, item);
        }

        #endregion --- SetChildren ---

        #region --- Get Children ---

        public bool ChildExists<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            return ChildExists<T>(controller,entityTypeID, 0);
        }

        public bool ChildExists<T>(ICommandController controller,IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_children.Exists<T>(controller,entityTypeIDVal, index);
        }

        public T GetChildEntity<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetChildEntity<T>(controller,entityTypeID, 0);
        }
        public T GetChildEntity<T>(ICommandController controller,IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_children.Get<T>(controller,entityTypeIDVal, index);
        }
        public IReadOnlyList<T> GetChildEntites<T>(ICommandController controller,IConvertible entityTypeID) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_children.Get<T>(controller,entityTypeIDVal).AsReadOnly();
        }
        public void AddChildEntity<T>(ICommandController controller,IEnumerable<T> items) where T : EntityBase
        {
            m_children.Add(controller,items);
        }
        public void AddChildEntity<T>(ICommandController controller,T item) where T : EntityBase
        {
            m_children.Add(controller,item);
        }
        #endregion --- Get Children ---

        #region --- Overrides ---

        protected override IDataCommand[] GetSaveReCacheCommands()
        {
            return new IDataCommand[] {
                EntityCommandFactory.LoadEntities(EntityTypeID),
                EntityCommandFactory.LoadEntity(EntityTypeID,Id )
            };
        }

        public override IDataCommand CreateDeleteCommand()
        {
            return EntityCommandFactory.Delete(EntityTypeID, Id);
        }

        protected override IDataCommand[] GetDeleteReCacheCommands()
        {
            return new IDataCommand[] {
                EntityCommandFactory.LoadEntities(EntityTypeID),
                EntityCommandFactory.LoadEntity(EntityTypeID, Id)
            };
        }
        protected override void OnLoad(IDataAdapter adapter)
        {
            m_lastUpdated = adapter.GetValue("LastUpdate", SqlDateTime.MinValue.Value);
            OnLoadFrom(adapter);
        }

        protected abstract void OnLoadFrom(IDataAdapter adapter);

        public virtual void AddParentEntity(ICommandController controller,EntityBase value)
        {
            m_parents.Add(controller,value);
        }

        protected override void OnSave(ICommandController controller)
        {
            foreach (EntityBase child in m_children)
            {
                child.Save(controller);
            }
        }

        protected virtual IDataCommand GetLoadSingleCommand(int id)
        {
            return EntityCommandFactory.LoadEntity(EntityTypeID, id);
        }

        protected virtual IDataCommand GetLoadCommand()
        {
            return EntityCommandFactory.LoadEntities(EntityTypeID);
        }
        #endregion --- Overrides ---

        #region --- Static Loaders ---

        public static IList<T> LoadEntities<T>(ICommandController controller) where T : EntityBase, new()
        {
            return Load<T>(controller.ExecuteCollectionCommand(new T().GetLoadCommand()));
        }
        public static T LoadEntity<T>(ICommandController controller,int id) where T : EntityBase, new()
        {
            return LoadSingle<T>(controller.ExecuteCollectionCommand(new T().GetLoadSingleCommand(id)));
        }

        public static T LoadChildEntity<T>(ICommandController controller,int parentEntityTypeID, long parentEntityID, int childEntityTypeID, int index) where T : EntityBase, new()
        {
            return LoadSingle<T>(controller.ExecuteCollectionCommand(EntityCommandFactory.LoadChildEntity(parentEntityTypeID, parentEntityID, childEntityTypeID, index)));
        }

        public static T LoadParentEntity<T>(ICommandController controller,int childEntityTypeID, long childEntityID, int parentEntityTypeID, int index) where T : EntityBase, new()
        {
            return LoadSingle<T>(controller.ExecuteCollectionCommand(EntityCommandFactory.LoadParentEntity(childEntityTypeID, childEntityID, parentEntityTypeID, index)));
        }

        public static IList<T> LoadParentEntities<T>(ICommandController controller,long entityID, int entityTypeID, int parentEntityTypeID) where T : EntityBase, new()
        {
            return Load<T>(controller.ExecuteCollectionCommand(EntityCommandFactory.LoadParentEntites(entityID, entityTypeID, parentEntityTypeID)));
        }

        public static IList<T> LoadChildEntities<T>(ICommandController controller,long entityID, int entityTypeID, int childEntityTypeID) where T : EntityBase, new()
        {
            return Load<T>(controller.ExecuteCollectionCommand(EntityCommandFactory.LoadChildEntites(entityID, entityTypeID, childEntityTypeID)));
        }

        #endregion --- Static Loaders ---
    }
}
