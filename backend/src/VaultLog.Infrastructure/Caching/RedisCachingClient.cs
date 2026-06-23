using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace VaultLog.Infrastructure.Caching;

public class RedisCachingClient
{
    public IConnectionMultiplexer Connection { get; }
    private readonly ILogger<RedisCachingClient> _logger;

    private readonly string _keyPrefix;
    
    public IDatabase Cache => Connection.GetDatabase();
    
    public RedisCachingClient(ILogger<RedisCachingClient> logger, IConnectionMultiplexer connectionMultiplexer, string prefix = "VaultLog")
    {
        _logger = logger;
        Connection = connectionMultiplexer;
        _keyPrefix = string.IsNullOrWhiteSpace(prefix) ? "" : $"{prefix.Trim(':')}:";
        AddCachingEvents();
    }
    
    public string FormatKey(string key) => $"{_keyPrefix}{key}";

    private void AddCachingEvents()
    {
        var serverConfig = Connection.Configuration;
        Connection.ErrorMessage += (_, args) =>
        {
            _logger.LogError("Redis server {Server} error at endpoint {EndPoint}: {Message}", serverConfig, args.EndPoint, args.Message);
        };
        
        Connection.ConnectionFailed += (_, args) =>
        {
            _logger.LogError(args.Exception, "Connection to Redis server {Server} failed. Failure type: {Type}", serverConfig, args.FailureType);
        };
        Connection.ConnectionRestored += (_, args) =>
        {
            _logger.LogInformation("Connection to Redis server {Server} successfully restored.", serverConfig);
        };
        Connection.InternalError += (_, args) =>
        {
            _logger.LogError(args.Exception, "Redis server {Server} encountered an internal exception.", serverConfig);
        };
    }
}