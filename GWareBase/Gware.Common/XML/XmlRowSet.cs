using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gware.Common.XML
{
    public class XmlRowSet
    {
        private string m_name;
        private string[] m_columns;

        private List<XmlRowSetRow> m_rows;

        public XmlRowSetRow this[int index]
        {
            get
            {
                return m_rows[index];
            }
        }
        public IEnumerable<XmlRowSetRow> Rows
        {
            get
            {
                return m_rows.AsReadOnly();
            }
        }
        public int RowCount
        {
            get
            {
                return m_rows.Count;
            }
        }
        public string Name
        {
            get { return m_name; }
        }

        public XmlRowSet(XmlNode node)
        {
            m_rows = new List<XmlRowSetRow>();
            LoadRowSet(node);
        }

        public void LoadRowSet(XmlNode node)
        {
            XmlAttribute nameAtt = node.Attributes["name"];
            m_name = nameAtt.Value;
            XmlAttribute columnsAtt = node.Attributes["columns"];
            m_columns = columnsAtt.Value.Split(',');
            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name.Equals("row"))
                    {
                        m_rows.Add(new XmlRowSetRow(this,childNode));
                    }
                }
            }
        }

        public override string ToString()
        {
            return String.Format("{XmlRowSet : {0} {1} rows {2} columns}", m_name, m_rows.Count,m_columns.Length);
        }
    }
}
