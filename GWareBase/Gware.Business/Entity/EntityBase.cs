using Gware.Business.Application;
using Gware.Business.Commands;
using Gware.Business.Entity.Collection;
using Gware.Common.Application;
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
            m_entityTypeID = GetType().GetEntityTypeID();
            m_parents = new StoredMultiEntityTypeCollection(this, EntityRelationship.Child);
            m_children = new StoredMultiEntityTypeCollection(this, EntityRelationship.Parent);
        }

        internal int GetID()
        {
            if(Id == 0)
            {
                Save();
            }

            return Id;
        }

        #region --- Set Parent ---

        public void SetParentEntity<T>(IConvertible entityTypeID, T item) where T : EntityBase, new()
        {
            SetParentEntity<T>(entityTypeID, 0, item);
        }
        public void SetParentEntity<T>(IConvertible entityTypeID, int index, T item) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            m_parents.Set<T>(entityTypeIDVal,index, item);
        }
        public void AddParentEntity<T>(IEnumerable<T> items) where T : EntityBase
        {
            m_parents.Add(items);
        }
        public void AddParentEntity<T>(T item) where T : EntityBase
        {
            m_parents.Add(item);
        }

        #endregion --- Set Parent ---

        #region --- Get Parent ---

        public bool ParentExists<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return ParentExists<T>(entityTypeID, -1);
        }

        public bool ParentExists<T>(IConvertible entityTypeID,int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_parents.Exists<T>(entityTypeIDVal, index);
        }

        public T GetOneToOneParentEntity<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntity<T>(entityTypeID, 0);
        }

        public T GetOneToManyParentEntity<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntity<T>(entityTypeID, -1);
        }
        
        public T GetParentEntity<T>(IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_parents.Get<T>(entityTypeIDVal, index);
        }
        public IReadOnlyList<T> GetParentEntites<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_parents.Get<T>(entityTypeIDVal).AsReadOnly();
        }

        #endregion --- Get Parent ---
        
        #region --- Set Children ---
        public void SetChildEntity<T>(IConvertible entityTypeID,T item) where T : EntityBase, new()
        {
            SetChildEntity<T>(entityTypeID,0,item);
        }
        public void SetChildEntity<T>(IConvertible entityTypeID, int index,T item) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            m_children.Set<T>(entityTypeIDVal,index,item);
        }

        #endregion --- SetChildren ---

        #region --- Get Children ---

        public bool ChildExists<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return ChildExists<T>(entityTypeID, 0);
        }

        public bool ChildExists<T>(IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_children.Exists<T>(entityTypeIDVal, index);
        }

        public T GetChildEntity<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetChildEntity<T>(entityTypeID,0);
        }
        public T GetChildEntity<T>(IConvertible entityTypeID,int index) where T : EntityBase, new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_children.Get<T>(entityTypeIDVal, index);
        }
        public IReadOnlyList<T> GetChildEntites<T>(IConvertible entityTypeID) where T : EntityBase,new()
        {
            int entityTypeIDVal = entityTypeID.ToInt32(CultureInfo.CurrentCulture);

            return m_children.Get<T>(entityTypeIDVal).AsReadOnly();
        }
        public void AddChildEntity<T>(IEnumerable<T> items) where T : EntityBase
        {
            m_children.Add(items);
        }
        public void AddChildEntity<T>(T item) where T : EntityBase
        {
            m_children.Add(item);
        }
        #endregion --- Get Children ---

        #region --- Overrides ---

        protected override IDataCommand[] GetSaveReCacheCommands()
        {
            return new IDataCommand[] {
                EntityCommandFactory.LoadEntities(EntityTypeID),
                EntityCommandFactory.LoadEntity(Id, EntityTypeID)
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
                EntityCommandFactory.LoadEntity(Id, EntityTypeID)
            };
        }
        protected override void OnLoad(IDataAdapter adapter)
        {
            m_lastUpdated = adapter.GetValue("LastUpdate", SqlDateTime.MinValue.Value);
            OnLoadFrom(adapter);
        }

        protected abstract void OnLoadFrom(IDataAdapter adapter);

        public virtual void AddParentEntity(EntityBase value)
        {
            m_parents.Add(value);
        }

        protected override void OnSave()
        {
            foreach(EntityBase child in m_children)
            {
                child.Save();
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

        public static IList<T> LoadEntities<T>() where T : EntityBase, new()
        {
            return Load<T>(EntityCommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(new T().GetLoadCommand()));
        }
        public static T LoadEntity<T>(int id) where T : EntityBase, new()
        {
            return LoadSingle<T>(EntityCommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(new T().GetLoadSingleCommand(id)));
        }

        public static T LoadChildEntity<T>(int parentEntityTypeID, int parentEntityID, int childEntityTypeID, int index) where T : EntityBase, new()
        {
            return LoadSingle<T>(EntityCommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadChildEntity(parentEntityTypeID, parentEntityID, childEntityTypeID, index)));
        }

        public static T LoadParentEntity<T>(int childEntityTypeID, int childEntityID, int parentEntityTypeID, int index) where T : EntityBase, new()
        {
            return LoadSingle<T>(EntityCommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadParentEntity(childEntityTypeID, childEntityID, parentEntityTypeID, index)));
        }

        public static IList<T> LoadParentEntities<T>(int entityID, int entityTypeID, int parentEntityTypeID) where T : EntityBase, new()
        {
            return Load<T>(EntityCommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadParentEntites(entityID, entityTypeID, parentEntityTypeID)));
        }

        public static IList<T> LoadChildEntities<T>(int entityID, int entityTypeID, int childEntityTypeID) where T : EntityBase, new()
        {
            return Load<T>(EntityCommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadChildEntites(entityID, entityTypeID, childEntityTypeID)));
        }

        #endregion --- Static Loaders ---
    }
}
