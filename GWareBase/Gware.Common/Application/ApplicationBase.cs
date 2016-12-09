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

namespace Gware.Common.Application
{
    public static class ApplicationBase
    {
        public static readonly Encoding c_ApplicationEncoding = Encoding.Unicode;
        public const bool c_useNetworkOrder = false;
        public static readonly Random c_ApplicationRandom = new Random();

        private const string c_settingsFileName = "app.config";
        private static readonly string m_applicationDataFolder;
        private static readonly string m_settingsFilePath;
        private static readonly string m_applicationUserFolder;
        private static readonly Assembly m_entryAssembly;

        private static AutoEncryptDecrypt m_encryptDecrypt;
        private static object m_encryptionCreationLock = new object();
        public static AutoEncryptDecrypt EncryptDecrypt
        {
            get 
            {
                lock (m_encryptionCreationLock) 
                {
                    if (m_encryptDecrypt == null)
                    {
                        m_encryptDecrypt = new AutoEncryptDecrypt(null);
                    }
                }
                return ApplicationBase.m_encryptDecrypt; 
            }
        }
        public static string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }
        public static string ApplicationDataFolder
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
        public static string ApplicationUserFolder
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
        public static string CompanyDataFolder
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
        public static string CompanyUserFolder
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
        public static string ApplicationTitle
        {
            get 
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                return entryAssembly.GetTitle();
                
             }
        }
        public static string ApplicationCompanyName
        {
            get 
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                return entryAssembly.GetCompany();
            }
        }
        public const int c_QueryScanInterval = 500;
        public const int c_QueryCancelTimer = 30000;
        static ApplicationBase()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                entryAssembly  = Assembly.GetCallingAssembly();
            }
            m_entryAssembly = entryAssembly;
            if (entryAssembly != null)
            {
                m_applicationDataFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), entryAssembly.GetCompany());
                m_applicationUserFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), entryAssembly.GetCompany());

                m_settingsFilePath = System.IO.Path.Combine(m_applicationDataFolder, c_settingsFileName);

                if (!System.IO.File.Exists(m_settingsFilePath))
                {
                    m_settingsFilePath = System.IO.Path.Combine(entryAssembly.Location,c_settingsFileName);
                }
            }
            
            LoadBase();

            m_connections.Set(DatabaseConnection.AuthenticationServer.ConnectionName, DatabaseConnection.AuthenticationServer);
            
        }

        private static Dictionary<string, DatabaseConnection> m_connections = new Dictionary<string, DatabaseConnection>();
        private static int m_maxExecutionThreads;

        public static int MaxExecutionThreads
        {
            get { return ApplicationBase.m_maxExecutionThreads; }
            set { ApplicationBase.m_maxExecutionThreads = value; }
        }
        
        public static DatabaseConnection GetConnection(string name)
        {
            if (m_connections.ContainsKey(name))
            {
                return m_connections[name];
            }
            return null;
        }

        public static void LoadBase()
        {
            if (System.IO.File.Exists(m_settingsFilePath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(m_settingsFilePath);
                XmlNode connectionsNode = doc.DocumentElement.GetNode("Connections");
                if (connectionsNode != null)
                {
                    XmlNodeList connectionNodes = connectionsNode.GetChildNodes("Connection");
                    foreach (XmlNode node in connectionNodes)
                    {
                        DatabaseConnection conn = new DatabaseConnection(node);
                        m_connections.Set(conn.ConnectionName, conn);
                    }
                    
                }

                m_maxExecutionThreads = doc.DocumentElement.GetAttributeInt("MaxConcurrentExecutables", 15);
            }

        }
        public static void SaveBase()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("config"));
            XmlNode connectionsNode = doc.DocumentElement.AddNode("Connections");

            foreach (string key in m_connections.Keys)
            {
                XmlNode connectionNode = connectionsNode.AddNode("Connection");
                m_connections[key].AppendTo(connectionNode);
            }

            doc.DocumentElement.AddAttribute("MaxConcurrentExecutables", m_maxExecutionThreads);

            doc.Save(m_settingsFilePath);
        }

        public static string GenerateAuthenticationToken()
        {
            byte[] tokenBytes = new byte[128];
            c_ApplicationRandom.NextBytes(tokenBytes);

            return Application.ApplicationBase.c_ApplicationEncoding.GetString(tokenBytes);
        }

    }
}
