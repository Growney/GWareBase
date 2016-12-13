using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Threading
{
    public class ActionThread : ThreadBase
    {
        private IExecuteable m_action = null;

        public event EventHandler<ActionThread> OnThreadComplete;

        protected override void ExecuteSingleThreadCycle()
        {
            if (m_action != null)
            {
                m_action.Execute();
            }
            Pause();
            if (OnThreadComplete != null)
            {
                OnThreadComplete(this, this);
            }
        }
        public override void Resume()
        {
            if (m_action != null)
            {
                m_action.Resume();
            }
            base.Resume();
        }
        public override void Pause()
        {
            if (m_action != null)
            {
                m_action.Pause();
            }
            base.Pause();
        }
        public void Cancel()
        {
            if (m_action != null)
            {
                m_action.Cancel();
            }
        }
        public virtual void DoWork(IExecuteable action)
        {
            m_action = action;
            Resume();
        }

    }
}
