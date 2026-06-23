using System.Text.Json;
using StackExchange.Redis;

namespace VaultLog.Infrastructure.Caching;

/// <summary>
/// A simple caching extension helper class to serialize and deserialize records to
/// the Redis cache. 
/// </summary>
public static class CachingExtensions
{
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(30);
    
    public static async Task AddRecordAsync<T>(this RedisCachingClient client, string key, T data, TimeSpan? ttl = null)
    {
        var prefixedKey = (RedisKey)client.FormatKey(key);
        var jsonData = JsonSerializer.Serialize(data);
        await client.Cache.StringSetAsync(prefixedKey, jsonData, ToExpiration(ttl));
    }

    public static async Task<T?> GetRecordAsync<T>(this RedisCachingClient client, string key)
    {
        var prefixedKey = (RedisKey)client.FormatKey(key);
        var redisValue = await client.Cache.StringGetAsync(prefixedKey);

        if (!redisValue.HasValue || redisValue.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(redisValue.ToString());
    }

    public static async Task RemoveRecordAsync(this RedisCachingClient client, string key)
    {
        var prefixedKey = (RedisKey)client.FormatKey(key);
        await client.Cache.KeyDeleteAsync(prefixedKey);
    }
    
    private static Expiration ToExpiration(TimeSpan? ttl) =>
        new Expiration(ttl ?? DefaultTtl);
}