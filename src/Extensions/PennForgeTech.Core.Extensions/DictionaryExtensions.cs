using System;
using System.Collections.Generic;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createValue)
            where TKey : notnull
        {
            Guard.AgainstNull(dictionary, nameof(dictionary));
            Guard.AgainstNull(createValue, nameof(createValue));

            if (dictionary.TryGetValue(key, out TValue? existingValue))
            {
                return existingValue;
            }

            TValue newValue = createValue();
            dictionary[key] = newValue;
            return newValue;
        }
    }
}
