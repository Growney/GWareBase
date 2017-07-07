using Gware.Business.Commands;
using Gware.Business.Entity.Collection;
using Gware.Common.Application;
using Gware.Common.Storage;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
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



        private MultiEntityTypeCollection m_parents;

        public EntityBase()
        {
            m_entityTypeID = GetType().GetEntityTypeID();
            m_parents = new MultiEntityTypeCollection(this,EntityRelationship.Parent);
        }
        public void SaveWithAssignmentToChildEntity(EntityBase child)
        {
            SaveParentWithChildAssignment(child, this);
        }
        public void SaveAssignmentToChildEntity(EntityBase child)
        {
            SaveAssignmentToParentEntity(child, this);
        }

        public void SaveWithAssignmentToParentEntity(EntityBase parent)
        {
            SaveChildWithParentAssignment(this, parent);
        }
        public void SaveAssignmentToParentEntity(EntityBase parent)
        {
            SaveAssignmentToParentEntity(this, parent);
        }
        protected override void OnSave()
        {
            foreach (EntityBase parent in m_parents)
            {
                SaveParentWithChildAssignment(this, parent);
            }
        }
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

        protected void SetParentEntity<T>(IConvertible entityTypeID, T value) where T : EntityBase
        {
            SetParentEntity<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture), value, 0);
        }
        protected void SetParentEntity<T>(IConvertible entityTypeID, T value, int index) where T : EntityBase
        {
            SetParentEntity<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture), value, index);
        }
        protected void SetParentEntity<T>(int entityTypeID, T value, int index) where T : EntityBase
        {
            m_parents.Set(index, entityTypeID, value);
        }
        protected T GetParentEntity<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntity<T>(entityTypeID, 0);
        }
        protected T GetParentEntity<T>(IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            return GetParentEntity<T>((int)entityTypeID, index);
        }
        protected T GetParentEntity<T>(int entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntity<T>(entityTypeID, 0);
        }
        protected T GetParentEntity<T>(int entityTypeID, int index) where T : EntityBase, new()
        {
            return m_parents.Get<T>(index, entityTypeID);
        }
        protected IReadOnlyList<T> GetParentEntites<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntites<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture));
        }
        protected IReadOnlyList<T> GetParentEntites<T>(int entityTypeID) where T : EntityBase, new()
        {
            return m_parents.Get<T>(entityTypeID);
        }

        protected virtual DataCommand GetLoadSingleCommand(int id)
        {
            return EntityCommandFactory.LoadEntity(EntityTypeID, id);
        }

        protected virtual DataCommand GetLoadCommand()
        {
            return EntityCommandFactory.LoadEntities(EntityTypeID);
        }
        public static void SaveChildWithParentAssignment(EntityBase childEntity, EntityBase parentEntity)
        {
            childEntity.Save();
            SaveAssignmentToParentEntity(childEntity, parentEntity);
        }
        public static void SaveAssignmentToParentEntity(EntityBase childEntity, EntityBase parentEntity)
        {
            EntityAssignment.Save(parentEntity.Id, parentEntity.EntityTypeID, childEntity.Id, childEntity.EntityTypeID);
        }
        public static void SaveParentWithChildAssignment(EntityBase child, EntityBase parent)
        {
            parent.Save();
            SaveAssignmentToParentEntity(child, parent);
        }
        public static List<T> LoadParentEntities<T>(int entityID, int entityTypeID, int parentEntityTypeID) where T : EntityBase, new()
        {
            return Load<T>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadParentEntitiesWithType(entityID, entityTypeID, parentEntityTypeID)));
        }
        public static List<T> LoadEntities<T>() where T : EntityBase, new()
        {
            return Load<T>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(new T().GetLoadCommand()));
        }
        public static T LoadEntity<T>(int id) where T : EntityBase, new()
        {
            return LoadSingle<T>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(new T().GetLoadSingleCommand(id)));
        }

    }
}
