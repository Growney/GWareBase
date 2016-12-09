using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gware.Common.XML
{
    public class XmlRowSetRow : XmlRowSetRowBase
    {
        private List<XmlRowSet> m_childRowSet  = new List<XmlRowSet>();
        private XmlRowSet m_parentRowSet;

        private XmlRowDetail m_detailRow;

        public XmlRowDetail Detail
        {
            get
            {
                return m_detailRow;
            }
        }

        public XmlRowSet this[string childName]
        {
            get
            {
                XmlRowSet retVal = null;
                for (int i = 0; i < m_childRowSet.Count; i++) 
                {
                    XmlRowSet rowSet = m_childRowSet[i];
                    if (rowSet.Name.Equals(childName))
                    {
                        retVal = rowSet;
                        break;
                    }
                }
                return retVal;
            }
        }

        public XmlRowSetRow(XmlRowSet parentRowSet,XmlNode node) : base(node)
        {
           m_parentRowSet = parentRowSet;
        }

        protected override void LoadFromNode(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name.Equals("rowset"))
                    {
                        m_childRowSet.Add(new XmlRowSet(childNode));
                    }
                    else
                    {
                        m_detailRow = new XmlRowDetail(childNode);
                    }
                }
            }
        }

        public override string ToString()
        {
            return String.Format("{XmlRowSetRow : {0} values {1} child rowsets {2}}", m_values.Count, m_childRowSet.Count, m_detailRow);
        }
        
    }
}
