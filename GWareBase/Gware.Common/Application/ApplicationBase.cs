using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Gware.Common.XML;
using Gware.Common.Database;
using Gware.Common.DataStructures;
using Gware.Common.Reflection;
using Gware.Common.Encryption;
using System.IO;
using Gware.Common.Logging;

namespace Gware.Common.Application
{
    public abstract class ApplicationBase
    {
        private readonly Encoding m_applicationEncoding = Encoding.Unicode;
        private bool m_useNetworkOrder = false;
        private readonly Random m_applicationRandom = new Random();

        private readonly string c_settingsFileName = "app.config";
        private readonly string m_applicationDataFolder;
        private readonly string m_settingsFilePath;
        private readonly string m_applicationUserFolder;
        private readonly Assembly m_entryAssembly;

        public string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }
        public string ApplicationDataFolder
        {
            get
            {
                string retVal = Path.Combine(m_applicationDataFolder, m_entryAssembly.GetProduct());
                if (!Directory.Exists(retVal))
                {
                    Directory.CreateDirectory(retVal);
                }
                return retVal;
            }
        }
        public string ApplicationUserFolder
        {
            get
            {
                string retVal = Path.Combine(m_applicationUserFolder, m_entryAssembly.GetProduct());
                if (!Directory.Exists(retVal))
                {
                    Directory.CreateDirectory(retVal);
                }
                return retVal;
            }
        }
        public string CompanyDataFolder
        {
            get
            {
                string retVal = m_applicationDataFolder;
                if (!Directory.Exists(retVal))
                {
                    Directory.CreateDirectory(retVal);
                }
                return retVal;
            }
        }
        public string CompanyUserFolder
        {
            get
            {
                string retVal = m_applicationUserFolder;
                if (!Directory.Exists(retVal))
                {
                    Directory.CreateDirectory(retVal);
                }
                return retVal;
            }
        }
        public string ApplicationTitle
        {
            get 
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                return entryAssembly.GetTitle();
                
             }
        }
        public string ApplicationCompanyName
        {
            get 
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                return entryAssembly.GetCompany();
            }
        }
        public Encoding ApplicationEncoding
        {
            get
            {
                return m_applicationEncoding;
            }
        }
        public bool UseNetworkOrder
        {
            get
            {
                return m_useNetworkOrder;
            }

            set
            {
                m_useNetworkOrder = value;
            }
        }
        public Random ApplicationRandom
        {
            get
            {
                return m_applicationRandom;
            }
        }
        public ApplicationBase(bool useNetworkOrder)
            : this()
        {
            m_useNetworkOrder = useNetworkOrder;
        }
        public ApplicationBase(Encoding applicationEncoding)
            : this()
        {
            m_applicationEncoding = applicationEncoding;
        }
        public ApplicationBase(Encoding applicationEncoding,bool useNetworkOrder)
            :this()
        {
            m_applicationEncoding = applicationEncoding;
            m_useNetworkOrder = useNetworkOrder;
        }
        public ApplicationBase()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                entryAssembly = Assembly.GetCallingAssembly();
            }
            m_entryAssembly = entryAssembly;
            if (entryAssembly != null)
            {
                m_applicationDataFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), entryAssembly.GetCompany());
                m_applicationUserFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), entryAssembly.GetCompany());

                m_settingsFilePath = System.IO.Path.Combine(m_applicationDataFolder, c_settingsFileName);

                if (!System.IO.File.Exists(m_settingsFilePath))
                {
                    m_settingsFilePath = System.IO.Path.Combine(entryAssembly.Location, c_settingsFileName);
                }
            }
        }
    
        public void LogException(MethodBase method, Exception ex)
        {
            ExceptionLogger.Logger.LogException(method, ex, ApplicationDataFolder);
        }
        public void LogString(string logString)
        {
            ExceptionLogger.Logger.LogString(logString, ApplicationDataFolder);
        }

    }
}
