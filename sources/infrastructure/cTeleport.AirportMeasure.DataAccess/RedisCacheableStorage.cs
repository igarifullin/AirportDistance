using System;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace cTeleport.AirportMeasure.DataAccess
{
    public class RedisCacheableStorage : ICacheableStorage
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;
        private readonly string _keyPrefix;
        private readonly int _dbIndex = -1;

        public RedisCacheableStorage(ConnectionStringsConfig config, string keyPrefix)
        {
            _keyPrefix = keyPrefix;
            var connectionString = config.RedisConnection;
            if (connectionString.Contains(";"))
            {
                var parts = connectionString.Split(';');
                if (parts.Length != 2)
                {
                    throw new ArgumentException($"Invalid connectionstring {connectionString}", nameof(connectionString));
                }

                if (!int.TryParse(parts[1], out int dbIndex))
                {
                    throw new ArgumentException($"Invalid connectionstring dbIndex {connectionString}", nameof(connectionString));
                }

                _dbIndex = dbIndex;
                connectionString = parts[0];
            }

            var configurationOption = ConfigurationOptions.Parse(connectionString);

            // give a chance for redis to reconnect if it failed
            if (!connectionString.Contains("abortConnect"))
            {
                configurationOption.AbortOnConnectFail = false;
                configurationOption.ConnectRetry = 5;
            }
            
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOption));
        }

        public T Get<T>(string key)
        {
            var db = GetDatabase();
            var dbValue = db.StringGet(GetStorageKey(key));
            if (!dbValue.HasValue)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(dbValue);
        }

        public void Remove(string key)
        {
            var db = GetDatabase();
            db.KeyDelete(GetStorageKey(key));
        }

        public T Upsert<T>(string key, T value, TimeSpan? expiry = null)
        {
            var db = GetDatabase();
            db.StringSet(GetStorageKey(key), JsonConvert.SerializeObject(value), expiry);
            return value;
        }

        private string GetStorageKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return $"{_keyPrefix.ToLower()}_{key.ToLower()}";
        }

        private IDatabase GetDatabase()
        {
            return _connection.Value.GetDatabase(_dbIndex);
        }
    }
}