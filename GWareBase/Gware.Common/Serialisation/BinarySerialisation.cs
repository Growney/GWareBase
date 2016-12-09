using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Serialisation
{
    public static class BinarySerialisation
    {
        public static byte[] SerialiseObject(object obj)
        {
            if (obj != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    try
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(stream, obj);
                        byte[] buffer = stream.GetBuffer();
                        return buffer;
                    }
                    catch (Exception ex)
                    {
                        Gware.Common.Logging.ExceptionLogger.Logger.LogException(MethodBase.GetCurrentMethod(), ex);
                        return null;
                    }
                }
            }
            return null;
        }
        public static object DeserialiseObject(byte[] bytes)
        {
            if (bytes.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    try
                    {
                        object obj = binaryFormatter.Deserialize(stream);
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        Gware.Common.Logging.ExceptionLogger.Logger.LogException(MethodBase.GetCurrentMethod(), ex);
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
