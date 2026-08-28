using MediatR;
using Microsoft.Extensions.Logging;
using UverTeaServerApp.Shared.Caching;

namespace UverTeaServerApp.Shared.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(
        ICacheService cacheService, 
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery cacheableQuery || cacheableQuery.BypassCache)
        {
            return await next();
        }

        var cacheKey = cacheableQuery.CacheKey;

        try
        {
            var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                _logger.LogInformation("[CACHE HIT] Returning cached data for key: {CacheKey}", cacheKey);
                return cachedResponse;
            }

            _logger.LogInformation("[CACHE MISS] No cache for key: {CacheKey}. Fetching from source...", cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[CACHE READ ERROR] Failed to fetch cache for key: {CacheKey}. Proceeding to handler.", cacheKey);
        }

        var response = await next();

        if (response != null)
        {
            try
            {
                await _cacheService.SetAsync(
                    cacheKey, 
                    response, 
                    cacheableQuery.SlidingExpiration, 
                    cancellationToken);

                _logger.LogInformation("[CACHE SET] Cached response for key: {CacheKey}", cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[CACHE WRITE ERROR] Failed to cache response for key: {CacheKey}", cacheKey);
            }
        }

        return response;
    }
}
