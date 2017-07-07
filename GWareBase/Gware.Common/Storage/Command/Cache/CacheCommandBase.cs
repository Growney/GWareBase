using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;

namespace Gware.Common.Storage.Command.Cache
{
    public abstract class CacheCommandBase : ICommandController
    {
        private IRecacheTrigger m_trigger;
        private ulong m_hit;
        private ulong m_miss;
        private ulong m_skip;
        private ICommandController m_controller;

        public CacheCommandBase(ICommandController controller) : this(controller,new ManualRecacheTrigger())
        {
        }
        public CacheCommandBase(ICommandController controller,IRecacheTrigger trigger)
        {
            m_controller = controller;
            m_trigger = trigger;
            InitialiseTrigger();
            
        }

        private void InitialiseTrigger()
        {
            if(m_trigger != null)
            {
                m_trigger.OnClearCacheTrigger += OnClearCacheTrigger;
                m_trigger.OnRecacheTrigger += OnRecacheTrigger;
            }
        }

        private void OnRecacheTrigger(object sender, IDataCommand command)
        {
            Recache(command);
        }

        private void OnClearCacheTrigger(object sender, EventArgs e)
        {
            ClearCache();
        }

        protected abstract void ClearCache();

        protected abstract void StoreCollection(IDataCommand command, IDataAdapterCollection group);
        protected abstract IDataAdapterCollection CheckForColllection(IDataCommand command);
        public IDataAdapterCollection ExecuteCollectionCommand(IDataCommand command)
        {
            IDataAdapterCollection results = null;
            if (command.Cache)
            {
                results = CheckForColllection(command);
            }

            if (results == null)
            {

                results = m_controller.ExecuteCollectionCommand(command);
                if (command.Cache)
                {
                    m_miss++;
                    Task.Factory.StartNew(x =>
                    {
                        StoreCollection(command, results);
                    }, TaskCreationOptions.None);
                }
                else
                {
                    m_skip++;
                }

            }
            else
            {
                m_hit++;
            }
            CheckForReCache(command);
            return results;
        }

        protected abstract void StoreGroup(IDataCommand command, IDataAdapterCollectionGroup group);
        protected abstract IDataAdapterCollectionGroup CheckForGroup(IDataCommand command);
        public IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command)
        {
            IDataAdapterCollectionGroup results = null;
            if (command.Cache)
            {
                results = CheckForGroup(command);
            }
            
            if (results == null)
            {
               
                results = m_controller.ExecuteGroupCommand(command);
                if (command.Cache)
                {
                    m_miss++;
                    Task.Factory.StartNew(x =>
                    {
                        StoreGroup(command, results);
                    }, TaskCreationOptions.None);
                }
                else
                {
                    m_skip++;
                }
                
            }
            else
            {
                m_hit++;
            }
            CheckForReCache(command);
            return results;
        }

        protected abstract void Recache(IDataCommand command);

        public int ExecuteQuery(IDataCommand command)
        {
            CheckForReCache(command);
            return m_controller.ExecuteQuery(command);
        }

        private void CheckForReCache(IDataCommand command)
        {
            if (command.TriggersReCache)
            {
                Task.Factory.StartNew(x =>
                {
                    foreach (IDataCommand recache in command.ReCacheCommands)
                    {
                        Recache(recache);
                    }
                }, TaskCreationOptions.None);
            }
        }
    }
}
