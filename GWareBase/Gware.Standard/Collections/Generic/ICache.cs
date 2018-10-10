using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Standard.Collections.Generic
{
    public enum eCacheOptions
    {
        Default,
        ForceReload,
        OnlyStored
    }
    public interface ICache<K,D> : IDisposable
    {
        TimeSpan KeepFor { get; set; }
        Func<K, Task<D>> Read { set; }
        Task<D> GetItem(K key, eCacheOptions options = eCacheOptions.Default);
    }
}
