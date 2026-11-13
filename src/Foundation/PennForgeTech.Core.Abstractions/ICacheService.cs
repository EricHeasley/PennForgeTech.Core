using System;

namespace PennForgeTech.Core.Abstractions
{
    public interface ICacheService
    {
        bool TryGet<TValue>(string key, out TValue? value);

        void Store<TValue>(string key, TValue value, TimeSpan lifetime)
            where TValue : notnull;

        TValue GetOrCreate<TValue>(string key, TimeSpan lifetime, Func<TValue> create)
            where TValue : notnull;

        bool Remove(string key);

        void Clear();
    }
}
