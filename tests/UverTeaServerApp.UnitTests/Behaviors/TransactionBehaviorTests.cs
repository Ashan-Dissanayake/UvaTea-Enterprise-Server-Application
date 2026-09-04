using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.UnitTests.Behaviors;

public class TransactionBehaviorTests
{
    public record SampleCommand(string Value) : IRequest<string>, ITransactionalRequest;
    public record SampleQuery(string Value) : IRequest<string>;

    private readonly Mock<IUnitOfWork> _uowMock;

    public TransactionBehaviorTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Handle_WhenRequestIsQuery_ShouldBypassTransaction()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<TransactionBehavior<SampleQuery, string>>>();
        var behavior = new TransactionBehavior<SampleQuery, string>(_uowMock.Object, loggerMock.Object);
        var query = new SampleQuery("Get");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("QueryResult");

        // Act
        var result = await behavior.Handle(query, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("QueryResult");
        _uowMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        _uowMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        _uowMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandSucceeds_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<TransactionBehavior<SampleCommand, string>>>();
        var behavior = new TransactionBehavior<SampleCommand, string>(_uowMock.Object, loggerMock.Object);
        var command = new SampleCommand("Save");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("Saved");

        // Act
        var result = await behavior.Handle(command, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("Saved");
        _uowMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCommandFails_ShouldRollbackAndRethrow()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<TransactionBehavior<SampleCommand, string>>>();
        var behavior = new TransactionBehavior<SampleCommand, string>(_uowMock.Object, loggerMock.Object);
        var command = new SampleCommand("Fail");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("DB error"));

        // Act
        var act = () => behavior.Handle(command, nextMock.Object, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("DB error");
        _uowMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        _uowMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

