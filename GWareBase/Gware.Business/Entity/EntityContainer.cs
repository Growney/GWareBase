using Gware.Business.Commands;
using Gware.Business.Entity.Collection;
using Gware.Common.Application;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Business.Entity
{
    public abstract class EntityContainer : EntityBase
    {
        private MultiEntityTypeCollection m_children;

        public EntityContainer()
        {
            m_children = new MultiEntityTypeCollection(this, EntityRelationship.Child);
        }
        public override IDataCommand CreateSaveCommand()
        {
            return EntityContainerCommandFactory.Save(Id, EntityTypeID);
        }
        protected override void OnSave()
        {
            foreach(EntityBase child in m_children)
            {
                SaveChildWithParentAssignment(child, this);
            }
        }
        public override bool Delete()
        {
            return EntityAssignment.Delete(Id, EntityTypeID);
        }
        protected override void OnLoadFrom(IDataAdapter adapter)
        {
            
        }

        public void AddChildEntity(EntityBase value)
        {
            m_children.Add(value);
        }
        
        protected void SetChildEntity<T>(IConvertible entityTypeID, T value) where T : EntityBase
        {
            SetChildEntity<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture), value, 0);
        }
        protected void SetChildEntity<T>(IConvertible entityTypeID, T value, int index) where T : EntityBase
        {
            SetChildEntity<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture), value, index);
        }
        protected void SetChildEntity<T>(int entityTypeID, T value, int index) where T : EntityBase
        {
            m_children.Set(index, entityTypeID, value);
            
        }

        protected T GetChildEntity<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetChildEntity<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture), 0);
        }
        protected T GetChildEntity<T>(IConvertible entityTypeID, int index) where T : EntityBase, new()
        {
            return GetChildEntity<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture), index);
        }
        protected T GetChildEntity<T>(int entityTypeID) where T : EntityBase, new()
        {
            return GetChildEntity<T>(entityTypeID, 0);
        }
        protected T GetChildEntity<T>(int entityTypeID, int index) where T : EntityBase, new()
        {
            return m_children.Get<T>(index, entityTypeID);
        }
        protected IReadOnlyList<T> GetChildEntites<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetChildEntites<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture));
        }
        protected IReadOnlyList<T> GetChildEntites<T>(int entityTypeID) where T : EntityBase, new()
        {
            return m_children.Get<T>(entityTypeID);
        }

        protected override DataCommand GetLoadSingleCommand(int id)
        {
            return EntityContainerCommandFactory.LoadSingle(id, EntityTypeID);
        }

        protected override DataCommand GetLoadCommand()
        {
            return EntityContainerCommandFactory.Load(EntityTypeID);
        }

        public static T LoadEntity<T>(int ID, int childLevels) where T : EntityContainer, new()
        {
            T retVal = LoadEntity<T>(ID);
            return retVal;
        }
        public static List<T> LoadEntities<T>(int childLevels) where T : EntityContainer, new()
        {
            List<T> retVal = LoadEntities<T>();
            return retVal;
        }
        public static List<T> LoadChildEntitties<T>(int entityID, int entityTypeID, int childEntityTypeID) where T : EntityBase, new()
        {
            return Load<T>(CommandControllerApplicationBase.Main.Controller.ExecuteCollectionCommand(EntityCommandFactory.LoadChildEntitiesWithType(entityID, entityTypeID, childEntityTypeID)));
        }


    }
}
