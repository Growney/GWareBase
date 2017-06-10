using Gware.Common.Storage.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Application
{
    public abstract class CommandControllerApplicationBase
    {
        private static object m_initLock = new object();
        private static CommandControllerApplicationBase m_main;
        public static CommandControllerApplicationBase Main
        {
            get
            {
                return m_main;
            }
        }
        public static void InitializeBase(CommandControllerApplicationBase appBase)
        {
            lock (m_initLock)
            {
                m_main = appBase;
            }
        }

        private ICommandController m_controller;
        public ICommandController Controller
        {
            get
            {
                return m_controller;
            }
            set
            {
                m_controller = value;
            }
        }

        public void Init(ICommandController controller)
        {
            Controller = controller;
        }

        public CommandControllerApplicationBase(ICommandController controller)
        {
            Init(controller);
        }
    }
}
