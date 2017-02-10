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
        public System.Xml.XmlNode m_node;

        public XmlDataAdapter(System.Xml.XmlNode node)
        {
            m_node = node;
        }

        public override string GetValue(string fieldName, string defaultValue)
        {
            return m_node.GetString(fieldName, defaultValue);
        }
    }
}
