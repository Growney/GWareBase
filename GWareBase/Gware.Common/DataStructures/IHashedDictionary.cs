namespace Gware.Common.DataStructures
{
    public interface IHashedDictionary<Key, Value>
    {
        void Add(Key key, Value value);
        void Clear();
        Value Get(Key key);
        void Remove(Key key);
    }
}