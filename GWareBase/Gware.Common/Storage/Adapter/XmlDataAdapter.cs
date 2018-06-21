using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gware.Common.XML;
using System.Xml;
using Gware.Common.Storage.Command.Interface;

namespace Gware.Common.Storage.Adapter
{
    public class XmlDataAdapter : DataAdapterBase
    {
        private System.Xml.XmlNode m_node;

        public XmlDataAdapter(ICommandController controller,System.Xml.XmlNode node)
            :base(controller)
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

        public override void SetValue(string field, IConvertible value)
        {
            m_node.Set(field, value.ToString());
        }

        public override IEnumerable<string> GetFields()
        {
            List<string> retVal = new List<string>();

            XmlNodeList nodes = m_node.ChildNodes;

            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode child = nodes[i];
                if(child.ParentNode == m_node)
                {
                    if (!retVal.Contains(child.Name))
                    {
                        retVal.Add(child.Name);
                    }
                }
            }

            return retVal;
        }
    }
}
