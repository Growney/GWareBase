using Gware.Business.Commands;
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

        private int m_parentLevelsLoaded;
        private Dictionary<int, ArrayList> m_parentEntities = new Dictionary<int, ArrayList>();
        private List<AllowedEntityAssignment> m_allowedParentAssignments = new List<AllowedEntityAssignment>();

        public int ParentLevelsLoaded
        {
            get
            {
                return m_parentLevelsLoaded;
            }
        }

        public EntityBase()
        {

        }
        public void SaveWithAssignmentToParentEntity(EntityBase parent)
        {
            SaveWithAssignmentToParentEntity(this, parent);
        }
        protected override void OnSave()
        {
            foreach (int entityType in m_parentEntities.Keys)
            {
                foreach (EntityBase parentEntity in m_parentEntities[entityType])
                {
                    SaveWithAssignmentToParentEntity(parentEntity);
                }
            }
        }
        public override DataCommand CreateDeleteCommand()
        {
            return EntityCommandFactory.Delete(GetClassEntityType(), Id);
        }
        protected override void OnLoad(IDataAdapter adapter)
        {
            m_lastUpdated = adapter.GetValue("LastUpdate", SqlDateTime.MinValue.Value);
            OnLoadFrom(adapter);
        }
        protected abstract void OnLoadFrom(IDataAdapter adapter);
        public virtual int GetClassEntityType()
        {
            int retVal = -1;

            object[] attributes = GetType().GetCustomAttributes(false);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is EntityTypeAttribute)
                {
                    retVal = (int)(attributes[i] as EntityTypeAttribute).EntityType;
                    break;
                }
            }
            return retVal;
        }

        public void LoadParentEntities()
        {
            LoadParentEntities(0);
        }
        public void LoadParentEntities(int levels)
        {
            m_parentLevelsLoaded = levels;
            IDataAdapterCollectionGroup data = CommandControllerApplicationBase.Main.Controller.ExecuteGroupCommand(EntityCommandFactory.LoadParentEntities(GetClassEntityType(), Id));
            LoadEntites(data, m_allowedParentAssignments, m_parentEntities, false);
            foreach (int entityType in m_parentEntities.Keys)
            {
                foreach (EntityBase parent in m_parentEntities[entityType])
                {
                    if (levels < 0 || levels > 1)
                    {
                        if (parent is EntityBase)
                        {
                            (parent as EntityBase).LoadParentEntities(levels - 1);
                        }
                    }
                }
            }
        }

        public int AddParentEntity(EntityBase value)
        {
            int entityTypeID = value.GetClassEntityType();
            if (!m_parentEntities.ContainsKey(entityTypeID))
            {
                m_parentEntities.Add(entityTypeID, new ArrayList());
            }
            return m_parentEntities[entityTypeID].Add(value);
        }

        protected void SetParentEntity<T>(T value) where T : EntityBase
        {
            SetParentEntity<T>(value, 0);
        }
        protected void SetParentEntity<T>(T value, int index) where T : EntityBase
        {
            SetParentEntity<T>(value.GetClassEntityType(), value, index);
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
            if (!m_parentEntities.ContainsKey(entityTypeID))
            {
                m_parentEntities.Add(entityTypeID, new ArrayList());
            }
            m_parentEntities[entityTypeID][index] = value;
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
            T retVal = default(T);

            if (!m_parentEntities.ContainsKey(entityTypeID))
            {
                m_parentEntities.Add(entityTypeID, new ArrayList(LoadParentEntities<T>(Id, GetClassEntityType(), entityTypeID)));
            }

            if (index >= 0
                && index < m_parentEntities[entityTypeID].Count
                && m_parentEntities[entityTypeID][index] is T)
            {
                retVal = m_parentEntities[entityTypeID][index] as T;
            }

            return retVal;
        }
        protected List<T> GetParentEntites<T>(IConvertible entityTypeID) where T : EntityBase, new()
        {
            return GetParentEntites<T>(entityTypeID.ToInt32(CultureInfo.CurrentCulture));
        }
        protected List<T> GetParentEntites<T>(int entityTypeID) where T : EntityBase, new()
        {
            List<T> retVal = new List<T>();

            if (!m_parentEntities.ContainsKey(entityTypeID))
            {
                m_parentEntities.Add(entityTypeID, new ArrayList(LoadParentEntities<T>(Id, GetClassEntityType(), entityTypeID)));
            }
            foreach (object obj in m_parentEntities[entityTypeID])
            {
                if (obj is T)
                {
                    retVal.Add((T)obj);
                }
            }
            return retVal;
        }

        protected virtual DataCommand GetLoadSingleCommand(int id)
        {
            return EntityCommandFactory.LoadEntity(GetClassEntityType(), id);
        }

        protected virtual DataCommand GetLoadCommand()
        {
            return EntityCommandFactory.LoadEntities(GetClassEntityType());
        }
        public static void SaveWithAssignmentToParentEntity(EntityBase childEntity, EntityBase parentEntity)
        {
            childEntity.Save();
            EntityAssignment.Save(parentEntity.Id, parentEntity.GetClassEntityType(), childEntity.Id, childEntity.GetClassEntityType());
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
        public static void LoadEntites(IDataAdapterCollectionGroup data, List<AllowedEntityAssignment> allowedAssigned, Dictionary<int, ArrayList> assignments, bool toFrom)
        {
            allowedAssigned.Clear();
            allowedAssigned.AddRange(Load<AllowedEntityAssignment>(data.Collections[0]));
            assignments.Clear();
            for (int i = 0; i < allowedAssigned.Count; i++)
            {
                int assignedEntityTypeID = toFrom ? allowedAssigned[i].ToEntityType : allowedAssigned[i].FromEntityType;
                if (data.Collections.Length > i + 1)
                {
                    if (!assignments.ContainsKey(assignedEntityTypeID))
                    {
                        assignments.Add(assignedEntityTypeID, new ArrayList());
                    }
                    assignments[assignedEntityTypeID].Clear();
                }

                assignments[assignedEntityTypeID].Clear();

                for (int j = 0; j < data.Collections[i + 1].Adapters.Length; j++)
                {
                    EntityBase entity = EntityFactory.CreateEntity(assignedEntityTypeID);
                    EntityBase nonDirty = EntityFactory.CreateEntity(assignedEntityTypeID);
                    entity.Load(data.Collections[i + 1].Adapters[j]);
                    nonDirty.Load(data.Collections[i + 1].Adapters[j]);
                    entity.SetNonDirtyState(nonDirty);
                    assignments[assignedEntityTypeID].Add(entity);
                }
            }
        }
    }
}
