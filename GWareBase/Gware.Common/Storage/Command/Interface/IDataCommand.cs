using Gware.Common.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command.Interface
{
    public interface IDataCommand : IEnumerable<IDataCommandParameter>
    {
        string Name { get; }
        string CommandMethod { get; }

        IDataCommandParameter this[string name] { get; }
        bool Success { get; set; }
        Exception Exception { get; set; }
        int ParameterCount { get; }
        bool Cache { get; set; }
        bool TriggersReCache { get; }
        List<IDataCommand> ReCacheCommands { get; }

        void AddReCacheCommand(IDataCommand command);
        void AddReCacheCommand(ICollection<IDataCommand> commands);
        IDataCommandParameter AddParameter(IDataCommandParameter param);
        IDataCommandParameter AddParameter(string name, System.Data.DbType dataType, System.Data.ParameterDirection direction);
        IDataCommandParameter AddParameter(string name, System.Data.DbType dataType);
        IDataCommandParameter AddParameter(string name,string dataTypeName);
        void SetParameter(string name, object value);
        object GetParameterValue(string name);
        IDataCommandParameter GetParameter(string name);
    }
}
