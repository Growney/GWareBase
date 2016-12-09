using Gware.Common.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Logging
{
    public class ExceptionLogger
    {
        private static ExceptionLogger m_logger;
        public static ExceptionLogger Logger
        {
            get
            {
                if (m_logger == null)
                {
                    m_logger = new ExceptionLogger();
                }
                return m_logger;
            }
        }

        private ExceptionLogger()
        {


        }

        public void LogException(MethodBase method, Exception ex)
        {
            StringBuilder logEntry = new StringBuilder();
            logEntry.AppendLine(String.Format("{0} {1}::{2}", DateTime.Now, method.Name, ex.GetType().Name));
            logEntry.AppendLine(ex.Message);
            StackTrace trace = new StackTrace();
            StackFrame[] frames = trace.GetFrames();
            foreach (StackFrame frame in frames)
            {
                logEntry.AppendLine(String.Format("{0}.{1}", frame.GetType().Name, frame.GetMethod().Name));
            }
            LogString(logEntry.ToString());
        }
        private void LogString(string logString)
        {
            lock (this)
            {
                using (StreamWriter writer = File.AppendText(GetFile()))
                {
                    writer.Write(logString);
                }
            }
        }
        private string GetFile()
        {
            Assembly executingAssembly = Assembly.GetEntryAssembly();
            string fileName = executingAssembly.FullName;
            fileName += ".txt";
            return Path.Combine(ApplicationBase.ApplicationDataFolder, fileName);
        }
    }
}
