using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using cTeleport.AirportMeasure.Core;

namespace cTeleport.AirportMeasure.Services.Storages.Impl
{
    public class MemoryCacheableStorage : ICacheableStorage
    {
        private static readonly TimeSpan DefaultTimeToLeave = TimeSpan.FromMinutes(5);
        private readonly ConcurrentDictionary<string, CacheableStorageDocument> _concurrentDictionary = new ConcurrentDictionary<string, CacheableStorageDocument>();

        public T Get<T>(string key)
        {
            if (!_concurrentDictionary.ContainsKey(key))
                return default;

            var document = _concurrentDictionary[key];
            if (document.ExpireDate < DateTime.UtcNow)
            {
                Remove(key);
                return default;
            }

            return (T) document.Value;
        }

        public void Remove(string key)
        {
            _concurrentDictionary.Remove(key, out _);
        }

        public T Upsert<T>(string key, T value, TimeSpan? expiry = null)
        {
            var document = new CacheableStorageDocument
            {
                ExpireDate = DateTime.UtcNow.Add(expiry ?? DefaultTimeToLeave),
                Value = value
            };
            _concurrentDictionary.AddOrUpdate(key, document, (x, y) => y);
            return value;
        }
    }
}