using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gware.Common.XML
{
    public class XmlRowDetail : XmlRowSetRowBase
    {
        private string m_name;
        public string Name
        {
            get
            {
                return m_name;
            }
        }
        public XmlRowDetail(XmlNode node) :base(node)
        {
        }

        protected override void LoadFromNode(XmlNode node)
        {
            m_name = node.Name;
        }

        public override string ToString()
        {
            return String.Format("{XmlRowDetal : {0} {1} details}", m_name, m_values.Count);
        }
    }
}
