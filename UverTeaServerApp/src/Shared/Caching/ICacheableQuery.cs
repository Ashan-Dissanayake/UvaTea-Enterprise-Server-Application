namespace UverTeaServerApp.Shared.Caching;

/// <summary>
/// Marker interface for MediatR queries that can be cached.
/// </summary>
public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan? SlidingExpiration { get; }
    bool BypassCache { get; }
}
