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
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, obj);
                    byte[] buffer = stream.GetBuffer();
                    return buffer;
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
                   
                    object obj = binaryFormatter.Deserialize(stream);
                    return obj;
                }
            }
            return null;
        }
    }
}
