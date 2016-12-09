using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gware.Common.XML
{
    public class XmlRowSetRowBase
    {
        protected Dictionary<string, string> m_values;

        public XmlRowSetRowBase(XmlNode node)
        {
            LoadValues(node);
            LoadFromNode(node);
        }

        private void LoadValues(XmlNode node)
        {
            m_values = node.GetAttributes();
        }

        protected virtual void LoadFromNode(XmlNode node)
        {

        }
        public string GetString(string fieldName)
        {
            return GetString(fieldName, String.Empty);
        }
        public string GetString(string fieldName, string defaultValue)
        {
            string retVal = defaultValue;

            if (m_values.ContainsKey(fieldName))
            {
                retVal = m_values[fieldName];
            }

            return retVal;
        }

        public Int32 GetInt(string fieldname)
        {
            return GetInt(fieldname, 0);
        }
        public Int32 GetInt(string fieldName, int defaultValue)
        {
            int retVal;
            if (Int32.TryParse(GetString(fieldName), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }

        public DateTime GetDateTime(string field)
        {
            return GetDateTime(field, DateTime.MinValue);
        }
        public DateTime GetDateTime(string field, DateTime defaultValue)
        {
            DateTime retVal;
            if (DateTime.TryParse(GetString(field), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }

        public Int64 GetLong(string fieldName)
        {
            return GetLong(fieldName, 0);
        }
        public Int64 GetLong(string fieldName, long defaultValue)
        {
            Int64 retVal;
            if (Int64.TryParse(GetString(fieldName), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }

        public Decimal GetDecimal(string fieldName)
        {
            return GetDecimal(fieldName, 0.0m);
        }
        public Decimal GetDecimal(string fieldname, Decimal defaultValue)
        {
            Decimal retVal;
            if (Decimal.TryParse(GetString(fieldname), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }

        public float GetFloat(string fieldName)
        {
            return GetFloat(fieldName, 0.0f);
        }
        public float GetFloat(string fieldName, float defaultValue)
        {
            float retVal;
            if (float.TryParse(GetString(fieldName), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }
        public Boolean GetBoolean(string fieldName)
        {
            return GetBoolean(fieldName, false); 
        }
        public Boolean GetBoolean(string fieldName, bool defaultvalue)
        {
            Boolean retVal;
            if (Boolean.TryParse(GetString(fieldName), out retVal))
            {
                return retVal;
            }
            try
            {
                int possibleBool = GetInt(fieldName, -1);
                if (possibleBool != -1)
                {
                    retVal = Convert.ToBoolean(possibleBool);
                }
            }
            catch
            {

            }

            return defaultvalue;
        }

        public byte GetByte(string fieldName)
        {
            return GetByte(fieldName, 0);
        }
        public byte GetByte(string fieldName, byte defaultValue)
        {
            byte retVal;
            if (byte.TryParse(GetString(fieldName), out retVal))
            {
                return retVal;
            }
            return defaultValue;
        }


    }
}
