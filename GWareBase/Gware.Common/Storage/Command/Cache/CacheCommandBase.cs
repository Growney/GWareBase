using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.Storage.Adapter;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Common.Storage.Command.Cache
{
    public abstract class CacheCommandBase : CacheCommandBase<ICommandController>
    {
        public CacheCommandBase(ICommandController controller) : this(controller, new ManualRecacheTrigger())
        {
        }
        public CacheCommandBase(ICommandController controller, IRecacheTrigger trigger)
            :base(controller,trigger)
        {

        }
    }

    public abstract class CacheCommandBase<T> : ICommandController where T : ICommandController
    {
        private IRecacheTrigger m_trigger;
        private ulong m_hit;
        private ulong m_miss;
        private ulong m_skip;
        private T m_controller;

        protected T Controller
        {
            get
            {
                return m_controller;
            }
        }

        public CacheCommandBase(T controller) : this(controller,new ManualRecacheTrigger())
        {
        }
        public CacheCommandBase(T controller,IRecacheTrigger trigger)
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
            return ExecuteCacheCommand<IDataAdapterCollection>(command, CheckForColllection, m_controller.ExecuteCollectionCommand, StoreCollection);
        }

        protected abstract void StoreGroup(IDataCommand command, IDataAdapterCollectionGroup group);
        protected abstract IDataAdapterCollectionGroup CheckForGroup(IDataCommand command);
        public IDataAdapterCollectionGroup ExecuteGroupCommand(IDataCommand command)
        {
            return ExecuteCacheCommand<IDataAdapterCollectionGroup>(command, CheckForGroup, m_controller.ExecuteGroupCommand, StoreGroup);
        }

        protected abstract void Recache(IDataCommand command);

        protected delegate K CheckMethod<K>(IDataCommand command);
        protected delegate void StoreMethod<K>(IDataCommand command,K item);

        protected Result ExecuteCacheCommand<Result>(IDataCommand command, CheckMethod<Result> check,CheckMethod<Result> execute,StoreMethod<Result> store)
        {
            Result results = default(Result);

            if (command.Cache)
            {
                results = check(command);
            }

            if(results == null)
            {
                results = execute(command);
                if (command.Cache)
                {
                    m_miss++;
                    
                    store(command, results);
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

        public int ExecuteQuery(IDataCommand command)
        {
            CheckForReCache(command);
            return m_controller.ExecuteQuery(command);
        }
        protected void CheckForReCache(IDataCommand command)
        {
            if (command.TriggersReCache)
            {
                foreach (IDataCommand recache in command.ReCacheCommands)
                {
                    Recache(recache);
                }
            }
        }
    }
}
