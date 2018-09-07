using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Command
{
    public interface IDataCommand : IEnumerable<IDataCommandParameter>
    {
        string Name { get; }
        string CommandMethod { get; }

        IDataCommandParameter this[string name] { get; }
        bool Success { get; set; }
        Exception Exception { get; set; }
        int ParameterCount { get; }
        
        IDataCommandParameter AddParameter(IDataCommandParameter param);
        IDataCommandParameter AddParameter(string name, System.Data.DbType dataType, System.Data.ParameterDirection direction);
        IDataCommandParameter AddParameter(string name, System.Data.DbType dataType);
        IDataCommandParameter AddParameter(string name, string dataTypeName);

        void SetParameter(string name, object value);
        object GetParameterValue(string name);
        IDataCommandParameter GetParameter(string name);
    }
}
