using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Caching
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, CacheEntry> _entries = new ConcurrentDictionary<string, CacheEntry>();
        private readonly IClock _clock;

        public MemoryCacheService(IClock clock)
        {
            this._clock = Guard.AgainstNull(clock, nameof(clock));
        }

        public bool TryGet<TValue>(string key, out TValue? value)
        {
            Guard.AgainstNullOrWhiteSpace(key, nameof(key));

            if (this._entries.TryGetValue(key, out CacheEntry? entry))
            {
                if (entry.ExpiresAt <= this._clock.UtcNow)
                {
                    this._entries.TryRemove(new KeyValuePair<string, CacheEntry>(key, entry));
                }
                else if (entry.Value is TValue typedValue)
                {
                    value = typedValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public void Store<TValue>(string key, TValue value, TimeSpan lifetime)
            where TValue : notnull
        {
            Guard.AgainstNullOrWhiteSpace(key, nameof(key));

            if (lifetime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, "Lifetime must be greater than zero.");
            }

            DateTimeOffset expiresAt = this._clock.UtcNow.Add(lifetime);
            this._entries[key] = new CacheEntry(value, expiresAt);
        }

        public TValue GetOrCreate<TValue>(string key, TimeSpan lifetime, Func<TValue> create)
            where TValue : notnull
        {
            Guard.AgainstNull(create, nameof(create));

            if (this.TryGet(key, out TValue? existingValue) && existingValue is not null)
            {
                return existingValue;
            }

            TValue createdValue = create();
            this.Store(key, createdValue, lifetime);
            return createdValue;
        }

        public bool Remove(string key)
        {
            Guard.AgainstNullOrWhiteSpace(key, nameof(key));
            return this._entries.TryRemove(key, out CacheEntry? _);
        }

        public void Clear()
        {
            this._entries.Clear();
        }

        private sealed class CacheEntry
        {
            public CacheEntry(object value, DateTimeOffset expiresAt)
            {
                this.Value = value;
                this.ExpiresAt = expiresAt;
            }

            public object Value { get; }

            public DateTimeOffset ExpiresAt { get; }
        }
    }
}
