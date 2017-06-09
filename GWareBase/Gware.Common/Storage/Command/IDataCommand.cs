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
        string Name { get; }
        string CommandMethod { get; }

        bool Success { get; set; }
        Exception Exception { get; set; }
        int ParameterCount { get; }


        DataCommandParameter AddParameter(DataCommandParameter param);
        DataCommandParameter AddParameter(string name, System.Data.DbType dataType, System.Data.ParameterDirection direction);
        DataCommandParameter AddParameter(string name, System.Data.DbType dataType);
        void SetParameter(string name, object value);
        object GetParameterValue(string name);
        DataCommandParameter GetParameter(string name);
        DataCommandParameter GetParameter(int index);
    }
}
