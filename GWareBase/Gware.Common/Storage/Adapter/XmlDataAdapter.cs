using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.XML;

namespace Gware.Common.Storage.Adapter
{
    public class XmlDataAdapter : DataAdapterBase
    {
        private System.Xml.XmlNode m_node;

        public XmlDataAdapter(System.Xml.XmlNode node)
        {
            m_node = node;
        }

        public override string GetValue(string fieldName, string defaultValue)
        {
            return m_node.GetString(fieldName, defaultValue);
        }

        public override byte[] GetValue(string fieldName, byte[] defaultValue)
        {
            return Encoding.Unicode.GetBytes(GetValue(fieldName, Encoding.Unicode.GetString(defaultValue)));
        }
    }
}
