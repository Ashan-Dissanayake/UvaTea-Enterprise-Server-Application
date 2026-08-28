using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.Shared.Caching;

namespace UverTeaServerApp.UnitTests.Behaviors;

public class CachingBehaviorTests
{
    public record RegularQuery(string Id) : IRequest<string>;

    public record CacheableTestQuery(string CacheKey, TimeSpan? SlidingExpiration, bool BypassCache)
        : IRequest<string>, ICacheableQuery;

    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<CachingBehavior<CacheableTestQuery, string>>> _loggerMock;

    public CachingBehaviorTests()
    {
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<CachingBehavior<CacheableTestQuery, string>>>();
    }

    [Fact]
    public async Task Handle_WhenRequestIsNotCacheable_ShouldBypassCaching()
    {
        // Arrange
        var regularLogger = new Mock<ILogger<CachingBehavior<RegularQuery, string>>>();
        var behavior = new CachingBehavior<RegularQuery, string>(_cacheServiceMock.Object, regularLogger.Object);
        var query = new RegularQuery("1");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("Response");

        // Act
        var result = await behavior.Handle(query, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("Response");
        _cacheServiceMock.Verify(c => c.GetAsync<string>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never);
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCacheHit_ShouldReturnCachedDataWithoutCallingNext()
    {
        // Arrange
        var behavior = new CachingBehavior<CacheableTestQuery, string>(_cacheServiceMock.Object, _loggerMock.Object);
        var query = new CacheableTestQuery("test-key", TimeSpan.FromMinutes(5), false);
        var nextMock = new Mock<RequestHandlerDelegate<string>>();

        _cacheServiceMock
            .Setup(c => c.GetAsync<string>("test-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync("CachedResult");

        // Act
        var result = await behavior.Handle(query, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("CachedResult");
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCacheMiss_ShouldExecuteHandlerAndStoreInCache()
    {
        // Arrange
        var behavior = new CachingBehavior<CacheableTestQuery, string>(_cacheServiceMock.Object, _loggerMock.Object);
        var query = new CacheableTestQuery("miss-key", TimeSpan.FromMinutes(10), false);
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("FreshResult");

        _cacheServiceMock
            .Setup(c => c.GetAsync<string>("miss-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        // Act
        var result = await behavior.Handle(query, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("FreshResult");
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("miss-key", "FreshResult", TimeSpan.FromMinutes(10), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenBypassCacheIsTrue_ShouldExecuteHandlerWithoutCheckingCache()
    {
        // Arrange
        var behavior = new CachingBehavior<CacheableTestQuery, string>(_cacheServiceMock.Object, _loggerMock.Object);
        var query = new CacheableTestQuery("bypass-key", TimeSpan.FromMinutes(5), true);
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("BypassedResult");

        // Act
        var result = await behavior.Handle(query, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("BypassedResult");
        _cacheServiceMock.Verify(c => c.GetAsync<string>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }
}
