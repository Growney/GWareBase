using System.Data;

namespace Gware.Common.Storage.Command.Interface
{
    public interface IDataCommandParameter
    {
        DbType DataType { get; set; }
        ParameterDirection Direction { get; set; }
        string Name { get; set; }
        object Value { get; set; }
        bool AnyValueInCache { get; set; }

        bool Equals(DataCommandParameter obj);
        string ToString(bool cache);
    }
}