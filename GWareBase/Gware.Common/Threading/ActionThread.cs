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
                m_action = null;
            }
            if (OnThreadComplete != null)
            {
                OnThreadComplete(this, this);
            }
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
            Trigger();
        }

    }
}
