using Gware.Standard.Storage.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Storage.Adapter
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
