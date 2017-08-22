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
        private bool m_any;

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
        public bool Any
        {
            get
            {
                return m_any;
            }
            set
            {
                m_any = value;
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
        public DataCommandParameter(string name, object value, DbType type,bool any)
            :this(name,value,type,ParameterDirection.Input,any)
        {

        }
        public DataCommandParameter(string name,object value,DbType type,ParameterDirection direction, bool any)
        {
            m_name = name;
            m_value = value;
            m_direction = ParameterDirection.Input;
            m_dataType = type;
        }
        
    }
}
