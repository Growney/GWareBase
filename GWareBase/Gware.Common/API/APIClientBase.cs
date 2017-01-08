using Gware.Common.API.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API
{
    public abstract class APIClientBase : IAPIClient
    {
        private ISessonManager m_sessonManager;

        public ISessonManager SessionManager
        {
            get
            {
                return m_sessonManager;
            }
        }

        public APIClientBase(ISessonManager manager)
        {
            m_sessonManager = manager;
        }
        
        public virtual bool CanConnect()
        {
            return true;
        }
        public abstract APIConnectionStatus GetConnectionStatus();

        
    }
}
