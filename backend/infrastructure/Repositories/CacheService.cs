using System;
using Application.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace infrastructure.Repositories;

public class CachService : ICacheService
{
    private IDatabase _db;

    public CachService()
    {
        ConfigureRedis();
    }

    private void ConfigureRedis()
    {
        _db = ConnectionHelper.Connection.GetDatabase();
    }
    public T GetData<T>(string key)
    {
        var value = _db.StringGet(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return default;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
    var settings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore  
    };

        var isSet =_db.StringSet(key, JsonConvert.SerializeObject(value,settings), expiryTime);
        return isSet;
    }

    public object RemoveData(string key)
    {
        bool _isKeyExist = _db.KeyExists(key);
        if (_isKeyExist == true)
        {
            return _db.KeyDelete(key);
        }
        return false;
    }

}

