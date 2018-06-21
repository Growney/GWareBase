using Gware.Common.Storage.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Storage.Adapter
{
    public interface IDataAdapter
    {
        ICommandController Controller { get; }
        IEnumerable<string> GetFields();

        T GetValue<T>(string v, T t) where T : IConvertible;
        byte[] GetValue(string field, byte[] defaultValue);

        void SetValue(string field, IConvertible value);
    }
}
