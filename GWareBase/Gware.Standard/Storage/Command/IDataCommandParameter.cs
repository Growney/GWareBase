using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gware.Standard.Storage.Command
{
    public interface IDataCommandParameter
    {
        DbType DataType { get; set; }
        string DataTypeName { get; set; }
        ParameterDirection Direction { get; set; }
        string Name { get; set; }
        object Value { get; set; }
    }
}
