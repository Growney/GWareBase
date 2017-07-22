using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command
{
    public class DataCommandParameter : IDataCommandParameter
    {
        private string m_name;
        private DbType m_dataType;
        private ParameterDirection m_direction;
        private object m_value;
        private bool m_anyValueInCache;

        public DbType DataType
        {
            get
            {
                return m_dataType;
            }

            set
            {
                m_dataType = value;
            }
        }
        public ParameterDirection Direction
        {
            get
            {
                return m_direction;
            }

            set
            {
                m_direction = value;
            }
        }
        public string Name
        {
            get
            {
                return m_name;
            }

            set
            {
                m_name = value;
            }
        }
        public object Value
        {
            get
            {
                return m_value;
            }

            set
            {
                m_value = value;
            }
        }
        public bool AnyValueInCache
        {
            get
            {
                return m_anyValueInCache;
            }
            set
            {
                m_anyValueInCache = value;
            }
        }
        public DataCommandParameter(string name)
            :this(name,null)
        {

        }
            
        public DataCommandParameter(string name,object value)
            :this(name,value,DbType.String)
        {
            
        }
        public DataCommandParameter(string name, object value, DbType type)
            :this(name,value,type,ParameterDirection.Input)
        {

        }
        public DataCommandParameter(string name, object value, DbType type, ParameterDirection direction)
            :this(name,value,type,direction,false)
        {

        }
        public DataCommandParameter(string name, object value, DbType type,bool anyValueInCache)
            :this(name,value,type,ParameterDirection.Input,anyValueInCache)
        {

        }
        public DataCommandParameter(string name,object value,DbType type,ParameterDirection direction, bool anyValueInCache)
        {
            m_name = name;
            m_value = value;
            m_direction = ParameterDirection.Input;
            m_dataType = type;
        }
        
        public string ToString(bool cache)
        {
            string retVal = string.Empty;
            if (!cache || !AnyValueInCache)
            {
                retVal = ToString();
            }
            return retVal;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Value);
        }

        public bool Equals(DataCommandParameter obj)
        {
            if(obj != null && obj.Value != null & m_value != null)
            {
                return m_name.Equals(obj.Name) && m_value.Equals(obj.Value);
            }
            else
            {
                if (m_name.Equals(obj.Name))
                {
                    return m_value == null && obj.Value == null;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
