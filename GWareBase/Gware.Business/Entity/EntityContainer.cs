using Gware.Business.Commands;
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

        private int m_entityContainerTypeID;
        private int m_childLevelsLoaded;
        private Dictionary<int, ArrayList> m_childEntities = new Dictionary<int, ArrayList>();
        private List<AllowedEntityAssignment> m_allowedChildAssignments = new List<AllowedEntityAssignment>();

        public int EntityContainerTypeID
        {
            get
            {
                return m_entityContainerTypeID;
            }

            set
            {
                m_entityContainerTypeID = value;
            }
        }

        public int ChildLevelsLoaded
        {
            get
            {
                if (m_childLevelsLoaded < 0)
                {
                    return 0 - m_childLevelsLoaded;
                }
                else
                {
                    return m_childLevelsLoaded;
                }
            }
        }

        public EntityContainer()
        {
            m_entityContainerTypeID = GetClassEntityType();
        }
        public override DataCommand CreateSaveCommand()
        {
            return EntityContainerCommandFactory.Save(Id, m_entityContainerTypeID);
        }
        protected override void OnSave()
        {
            int parentEntityTypeID = GetClassEntityType();
            foreach (int entityType in m_childEntities.Keys)
            {
                foreach (EntityBase entityBase in m_childEntities[entityType])
                {
                    entityBase.Save();
                    EntityAssignment.Save(Id, parentEntityTypeID, entityBase.Id, entityBase.GetClassEntityType());
                }
            }
        }
        public override bool Delete()
        {
            int parentEntityTypeID = GetClassEntityType();
            foreach (int entityType in m_childEntities.Keys)
            {
                foreach (EntityBase entityBase in m_childEntities[entityType])
                {
                    entityBase.Delete();
                    EntityAssignment.Delete(Id, parentEntityTypeID, entityBase.Id, entityBase.GetClassEntityType());
                }
            }
            return base.Delete();
        }
        protected override void OnLoadFrom(IDataAdapter adapter)
        {
            m_entityContainerTypeID = adapter.GetValue("EntityContainerTypeID", -1);
        }
        public void LoadChildEntities()
        {
            LoadChildEntities(0);
        }
        public void LoadChildEntities(int levels)
        {
            m_childLevelsLoaded = levels;
            IDataAdapterCollectionGroup data = CommandControllerApplicationBase.Main.Controller.ExecuteGroupCommand(EntityCommandFactory.LoadChildEntities(GetClassEntityType(), Id));
            LoadEntites(data, m_allowedChildAssignments, m_childEntities, true);
            foreach (int entityType in m_childEntities.Keys)
            {
                foreach (EntityBase child in m_childEntities[entityType])
                {
                    if (levels < 0 || levels > 0)
                    {
                        if (child is EntityContainer)
                        {
                            (child as EntityContainer).LoadChildEntities(levels - 1);
                        }
                    }
                }
            }
        }

        public int AddChildEntity(EntityBase value)
        {
            int entityTypeID = value.GetClassEntityType();
            if (!m_childEntities.ContainsKey(entityTypeID))
            {
                m_childEntities.Add(entityTypeID, new ArrayList());
            }
            return m_childEntities[entityTypeID].Add(value);
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
            if (!m_childEntities.ContainsKey(entityTypeID))
            {
                m_childEntities.Add(entityTypeID, new ArrayList());
            }
            if(index >=0 && index < m_childEntities[entityTypeID].Count)
            {
                m_childEntities[entityTypeID][index] = value;
            }
            else
            {
                if(index >= 0)
                {
                    m_childEntities[entityTypeID].Insert(index, value);
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            
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
            T retVal = default(T);
            if (!m_childEntities.ContainsKey(entityTypeID))
            {
                m_childEntities.Add(entityTypeID, new ArrayList(LoadChildEntitties<T>(Id, GetClassEntityType(), entityTypeID)));
            }

            if (index >= 0
                && index < m_childEntities[entityTypeID].Count
                && m_childEntities[entityTypeID][index] is T)
            {
                retVal = m_childEntities[entityTypeID][index] as T;
            }
            else
            {
                retVal = new T();
                retVal.SetNonDirtyState(new T());
                if (!m_childEntities.ContainsKey(entityTypeID))
                {
                    m_childEntities.Add(entityTypeID, new ArrayList());
                }
                m_childEntities[entityTypeID].Add(retVal);
            }
            return retVal;
        }
        protected IReadOnlyList<T> GetChildEntites<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetChildEntites<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture));
        }
        protected IReadOnlyList<T> GetChildEntites<T>(int entityTypeID) where T : EntityBase, new()
        {
            List<T> retVal = new List<T>();

            if (!m_childEntities.ContainsKey(entityTypeID))
            {
                m_childEntities.Add(entityTypeID, new ArrayList(LoadChildEntitties<T>(Id, GetClassEntityType(), entityTypeID)));
            }
            foreach (object obj in m_childEntities[entityTypeID])
            {
                if (obj is T)
                {
                    retVal.Add((T)obj);
                }
            }
            return retVal;
        }

        protected override DataCommand GetLoadSingleCommand(int id)
        {
            return EntityContainerCommandFactory.LoadSingle(id, GetClassEntityType());
        }

        protected override DataCommand GetLoadCommand()
        {
            return EntityContainerCommandFactory.Load(GetClassEntityType());
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
