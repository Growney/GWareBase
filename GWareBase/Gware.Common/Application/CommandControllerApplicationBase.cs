using Gware.Common.Storage.Command;
using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Application
{
    public abstract class CommandControllerApplicationBase : CommandControllerApplicationBase<ICommandController>
    {
        public CommandControllerApplicationBase(ICommandController controller) : base(controller)
        {
        }
    }

    public abstract class CommandControllerApplicationBase<T> where T :ICommandController
    {
        private static object m_initLock = new object();
        private static CommandControllerApplicationBase<T> m_main;
        public static CommandControllerApplicationBase<T> Main
        {
            get
            {
                return m_main;
            }
        }
        public static void InitializeBase(CommandControllerApplicationBase<T> appBase)
        {
            lock (m_initLock)
            {
                m_main = appBase;
                m_main.OnBaseInitialize();
            }
        }

        private T m_controller;
        public T Controller
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

        public void Init(T controller)
        {
            Controller = controller;
        }

        public virtual void OnBaseInitialize()
        {

        }
        public CommandControllerApplicationBase(T controller)
        {
            Init(controller);
        }
    }
}
