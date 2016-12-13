using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gware.Common.XML
{
    public static class ExtensionMethods
    {
        public static byte[] GetBytes(this XmlDocument doc)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    doc.Save(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
        public static void Load(this XmlDocument doc, byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                doc.Load(ms);
            }
        }
        public static int GetInt(this XmlNode node,int defaultValue)
        {
            return node.GetInt(node.Name,defaultValue);
        }
        public static int GetInt(this XmlNode node, string fieldName, int defaultValue)
        {
            int retVal = defaultValue;
            if (Int32.TryParse(node.GetString(fieldName, defaultValue.ToString()), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static DateTime GetDateTime(this XmlNode node, DateTime defaultValue)
        {
            return node.GetDateTime(node.Name,defaultValue);
        }
        public static DateTime GetDateTime(this XmlNode node, string fieldName, DateTime defaultValue)
        {
            DateTime retVal = defaultValue;
            if(DateTime.TryParse(node.GetString(fieldName, String.Empty), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static long GetLong(this XmlNode node,long defaultValue)
        {
            return node.GetLong(node.Name,defaultValue);
        }
        public static long GetLong(this XmlNode node, string fieldName, long defaultValue)
        {
            long retVal = defaultValue;
            if(Int64.TryParse(node.GetString(fieldName, String.Empty), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static bool GetBoolean(this XmlNode node,bool defaultValue)
        {
            return node.GetBoolean(node.Name,defaultValue);
        }
        public static bool GetBoolean(this XmlNode node, string fieldName, bool defaultValue)
        {
            bool retVal = defaultValue;
            if(Boolean.TryParse(node.GetString(fieldName, String.Empty), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static string GetString(this XmlNode node,string defaultValue)
        {
            return node.GetString(node.Name,defaultValue);
        }
        public static string GetString(this XmlNode node, string fieldName, string defaultValue)
        {
            string retVal = defaultValue;
            try
            {
                XmlNode textNode = node.GetNode(fieldName);
                if (textNode != null)
                {
                    return textNode.InnerText;
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        public static XmlNode GetNode(this XmlNode node, string fieldName, int nodeIndex = 0)
        {
            XmlNode retVal = null;
            try
            {
                if(node.Name.Equals(fieldName))
                {
                    retVal = node;
                }else
                {
                    XmlElement element = (XmlElement)node;
                    XmlNodeList nodeList = element.GetElementsByTagName(fieldName);
                    if (nodeIndex >= 0 && nodeIndex < nodeList.Count)
                    {
                        retVal = nodeList[nodeIndex];
                    }
                }
               
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        public static XmlAttribute AddAttribute(this XmlNode node, string attributeName, object value)
        {
            return node.AddAttribute(node.Name, attributeName, value);
        }
        public static XmlAttribute AddAttribute(this XmlNode node, string fieldName, string attributeName, object value)
        {
            XmlAttribute retVal = node.Attributes.Append(node.OwnerDocument.CreateAttribute(attributeName));
            if (value != null)
            {
                retVal.Value = value.ToString();
            }
            return retVal;
        }
        public static XmlNode AddNode(this XmlNode node, string fieldName)
        {
            return node.AddNode(fieldName, null);
        }
        public static XmlNode AddNode(this XmlNode node, string fieldName, object value)
        {
            try
            {
                XmlNode retVal = node.AppendChild(node.OwnerDocument.CreateElement(fieldName));
                if (value != null)
                {
                    retVal.InnerText = value.ToString();
                }

                return retVal;
            }
            catch (Exception)
            {

            }
            return null;
        }
        public static XmlNode Set(this XmlNode node, string fieldName,object value)
        {
            XmlNode childNode = node.GetNode(fieldName);
            if (childNode != null)
            {
                if (value != null)
                {
                    childNode.Value = value.ToString();
                }
                else
                {
                    childNode.Value = string.Empty;
                }
            }
            else
            {
                childNode = node.AddNode(fieldName, value);
            }
            return childNode;
            
        }
        public static XmlAttribute SetAttribute(this XmlNode node, string attributeName, object value)
        {
            return node.SetAttribute(node.Name, attributeName, value);
        }
        public static XmlAttribute SetAttribute(this XmlNode node, string fieldName, string attributeName, object value)
        {
            XmlAttribute retVal = node.GetAttribute(fieldName,attributeName);
            if (retVal != null)
            {
                if (value != null)
                {
                    retVal.Value = value.ToString();
                }
                else
                {
                    retVal.Value = string.Empty;
                }
            }
            else
            {
                retVal = node.AddAttribute(fieldName, attributeName, value);
            }
            return retVal;
        }
        public static XmlNodeList GetChildNodes(this XmlNode node, string fieldName)
        {
            return ((XmlElement)node).GetElementsByTagName(fieldName);
        }
        public static Dictionary<string, string> GetAttributes(this XmlNode node)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            foreach (XmlAttribute att in node.Attributes)
            {
                if (!retVal.ContainsKey(att.Name))
                {
                    retVal.Add(att.Name, att.Value);
                }
            }
            return retVal;
        }
        
        public static XmlAttribute GetAttribute(this XmlNode node, string attributeName)
        {
            return node.GetAttribute(node.Name, attributeName);
        }
        public static XmlAttribute GetAttribute(this XmlNode node, string field, string attributeName)
        {
            XmlAttribute att = null;
            XmlNode childNode = GetNode(node, field);
            if (childNode != null)
            {
                att = childNode.Attributes[attributeName];
            }
            return att;
        }

        public static string GetAttributeString(this XmlNode node, string attributeName, string defaultValue)
        {
            return node.GetAttributeString(node.Name, attributeName, defaultValue);
        }
        public static string GetAttributeString(this XmlNode node, string field, string attributeName,string defaultValue)
        {
            string retVal = defaultValue;
            try
            {
                XmlAttribute att = node.GetAttribute(field, attributeName);
                if (att != null)
                {
                    return att.Value;
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        public static bool GetAttributeBoolean(this XmlNode node, string attributeName, bool defaultValue)
        {
            return node.GetAttributeBoolean(node.Name, attributeName, defaultValue);
        }
        public static bool GetAttributeBoolean(this XmlNode node, string field, string attributeName, bool defaultValue)
        {
            bool retVal = defaultValue;
            if (Boolean.TryParse(node.GetAttributeString(field, attributeName, String.Empty), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static long GetAttributeLong(this XmlNode node, string attributeName, long defaultvalue)
        {
            return node.GetAttributeLong(node.Name, attributeName, defaultvalue);
        }
        public static long GetAttributeLong(this XmlNode node, string field, string attributeName, long defaultValue)
        {
            long retVal = defaultValue;
            if (Int64.TryParse(node.GetAttributeString(field, attributeName, String.Empty), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static DateTime GetAttributeDateTime(this XmlNode node,string attributeName, DateTime defaultValue)
        {
            return node.GetAttributeDateTime(node.Name,attributeName, defaultValue);
        }
        public static DateTime GetAttributeDateTime(this XmlNode node, string fieldName,string attributeName, DateTime defaultValue)
        {
            DateTime retVal = defaultValue;
            if (DateTime.TryParse(node.GetAttributeString(fieldName,attributeName, String.Empty), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public static int GetAttributeInt(this XmlNode node,string attributeName ,int defaultValue)
        {
            return node.GetAttributeInt(node.Name,attributeName, defaultValue);
        }
        public static int GetAttributeInt(this XmlNode node, string fieldName,string attributeName, int defaultValue)
        {
            int retVal = defaultValue;
            if (Int32.TryParse(node.GetAttributeString(fieldName, defaultValue.ToString()), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
    } 
}
