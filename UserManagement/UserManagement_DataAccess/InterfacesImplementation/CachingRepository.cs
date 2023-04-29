using StackExchange.Redis;
using System.Text.Json;
using Usermanagement_Domain.Interfaces;

namespace UserManagement_DataAccess.InterfacesImplementation
{
    public class CachingRepository : ICaching
    {
        private IDatabase _cacheDb;

        public CachingRepository()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public bool RemoveData<T>(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if(exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }
    }
}
