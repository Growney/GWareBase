using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gware.Common.XML
{
    public class XmlResultSet
    {
        private Dictionary<string, string> m_dataNodes;
        private List<XmlRowSet> m_rowSets;

        
        public XmlRowSet this[string name]
        {
            get
            {
                XmlRowSet retVal = null;
                for(int i =0;i < m_rowSets.Count;i ++)
                {
                    XmlRowSet rowSet = m_rowSets[i];
                    if (rowSet.Name.Equals(name))
                    {
                        retVal = rowSet;
                        break;
                    }
                }
                return retVal;
            }
        }

        public XmlResultSet(XmlNode resultNode)
        {
            m_dataNodes = new Dictionary<string, string>();
            m_rowSets = new List<XmlRowSet>();
            LoadFromXmlNode(resultNode);
        }

        private void LoadFromXmlNode(XmlNode node)
        {
            if(node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name.Equals("rowset"))
                    {
                        m_rowSets.Add(new XmlRowSet(childNode));
                    }
                    else
                    {
                        if (!m_dataNodes.ContainsKey(childNode.Name))
                        {
                            m_dataNodes.Add(childNode.Name, childNode.InnerText);
                        }
                    }
                }
            }
        }

        public static XmlResultSet GetDocumentResults(XmlDocument doc)
        {
            return GetDocumentResults((XmlNode)doc);
            
        }
        public static XmlResultSet GetDocumentResults(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name.Equals("result"))
                    {
                        return new XmlResultSet(childNode);
                    }
                    else
                    {
                        XmlResultSet retVal = GetDocumentResults(childNode);
                        if (retVal != null)
                        {
                            return retVal;
                        }
                    }
                }
            }
            return null;
        }

        public override string ToString()
        {
            return String.Format("{XmlResultSet : {0} data rows {1} rowsets }", m_dataNodes.Count, m_rowSets.Count);
        }
    }
}
