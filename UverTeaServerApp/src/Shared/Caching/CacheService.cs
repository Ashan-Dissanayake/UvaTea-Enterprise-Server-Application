using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UverTeaServerApp.Shared.Caching;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CacheService> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IConfiguration _configuration;

    public CacheService(
        IDistributedCache distributedCache, 
        ILogger<CacheService> logger,
        IConfiguration configuration)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _configuration = configuration;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedData = await _distributedCache.GetAsync(key, cancellationToken);
            if (cachedData == null || cachedData.Length == 0)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve cache for key {CacheKey}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(
        string key, 
        T value, 
        TimeSpan? expiration = null, 
        CancellationToken cancellationToken = default)
    {
        if (value == null) return;

        try
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonSerializerOptions);
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetSlidingExpiration(expiration.Value);
            }
            else
            {
                // Default sliding expiration of 15 minutes
                options.SetSlidingExpiration(TimeSpan.FromMinutes(15));
            }

            await _distributedCache.SetAsync(key, bytes, options, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to set cache for key {CacheKey}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to remove cache for key {CacheKey}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Invalidating cache with prefix: {Prefix}", prefix);

        try
        {
            var redisConnectionString = _configuration.GetConnectionString("Redis");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                var redis = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(redisConnectionString);
                var endpoints = redis.GetEndPoints();
                
                foreach (var endpoint in endpoints)
                {
                    var server = redis.GetServer(endpoint);
                    var keys = server.Keys(pattern: prefix + "*");

                    foreach (var key in keys)
                    {
                        await _distributedCache.RemoveAsync(key!, cancellationToken);
                    }
                }
            }
            else
            {
                // Fallback if not using Redis or if clearing prefix is not possible
                // In a development/MemoryCache environment, we might just have to clear everything 
                // but IDistributedCache doesn't support Clear() either.
                // For now, we rely on Redis implementation which is the target for this template.
                _logger.LogWarning("RemoveByPrefixAsync called but Redis connection string is missing.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while removing cache by prefix: {Prefix}", prefix);
        }
    }
}
