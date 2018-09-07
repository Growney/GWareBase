using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gware.Standard.Storage.Command
{
    public class DataCommandParameter : IDataCommandParameter
    {
        public DbType DataType { get; set; }
        public string DataTypeName { get; set; }
        public ParameterDirection Direction { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }


        public DataCommandParameter(string name)
            : this(name, null)
        {

        }

        public DataCommandParameter(string name, object value)
            : this(name, value, DbType.String)
        {

        }
        public DataCommandParameter(string name, object value, DbType type)
            : this(name, value, type, ParameterDirection.Input)
        {

        }
        public DataCommandParameter(string name, object value, string dataTypeName)
        {
            Name = name;
            Value = value;
            DataTypeName = dataTypeName;
            DataType = DbType.Object;
        }
        public DataCommandParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            Name = name;
            Value = value;
            Direction = ParameterDirection.Input;
            DataType = type;
        }
    }
}
