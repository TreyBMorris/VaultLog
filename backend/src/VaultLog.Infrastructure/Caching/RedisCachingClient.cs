using StackExchange.Redis;

namespace VaultLog.Infrastructure.Caching;

/// <summary>
///     A lightweight wrapper around <see cref="IConnectionMultiplexer" /> that provides
///     prefixed key formatting and exposes the underlying redis database for cache operations.
/// </summary>
/// <param name="connectionMultiplexer">The StackExchange.Redis connection multiplexer.</param>
/// <param name="prefix">The key prefix applied to all cache entries. Defaults to "VaultLog"</param>
public class RedisCachingClient(
    IConnectionMultiplexer connectionMultiplexer,
    string prefix = "VaultLog")
{
    private readonly string _keyPrefix = string.IsNullOrWhiteSpace(prefix) ? "" : $"{prefix.Trim(':')}:";

    /// <summary>
    ///     The underlying <see cref="IConnectionMultiplexer" /> instance.
    /// </summary>
    public IConnectionMultiplexer Connection { get; } = connectionMultiplexer;

    /// <summary>
    ///     The Redis database instance used for cache read/write operations.
    /// </summary>
    public IDatabase Cache => Connection.GetDatabase();

    /// <summary>
    ///     Helper method to format a cache key by adding the prefix.
    /// </summary>
    /// <param name="key">The raw cache key.</param>
    /// <returns>The prefixed cache key ("VaultLog:key")</returns>
    public string FormatKey(string key)
        => $"{_keyPrefix}{key}";
}