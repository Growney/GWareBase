using Gware.Common.Storage.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Command
{
    public interface IDataCommand
    {
        void AddParameter(string name, System.Data.DbType dataType, System.Data.ParameterDirection direction);
        void AddParameter(string name, System.Data.DbType dataType);
        void SetParameter(string name, object value);

        IDataAdapterCollectionGroup ExecuteCommandGroup();
        IDataAdapterCollection ExecuteCommand();
        bool ExecuteQuery();
    }
}
