using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using UverTeaServerApp.Shared.Behaviors;

namespace UverTeaServerApp.UnitTests.Behaviors;

public class ValidationBehaviorTests
{
    public record SampleCommand(string Name) : IRequest<string>;

    [Fact]
    public async Task Handle_WhenNoValidators_ShouldCallNextAndReturnResponse()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<SampleCommand>>();
        var behavior = new ValidationBehavior<SampleCommand, string>(validators);
        var request = new SampleCommand("Test");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("Success");

        // Act
        var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("Success");
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidationPasses_ShouldCallNextAndReturnResponse()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<SampleCommand>>();
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<SampleCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var behavior = new ValidationBehavior<SampleCommand, string>(new[] { validatorMock.Object });
        var request = new SampleCommand("Test");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();
        nextMock.Setup(n => n(It.IsAny<CancellationToken>())).ReturnsAsync("Success");

        // Act
        var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be("Success");
        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationExceptionAndNotCallNext()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new("Name", "Name is required")
        };
        var validatorMock = new Mock<IValidator<SampleCommand>>();
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<SampleCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var behavior = new ValidationBehavior<SampleCommand, string>(new[] { validatorMock.Object });
        var request = new SampleCommand("");
        var nextMock = new Mock<RequestHandlerDelegate<string>>();

        // Act
        var act = () => behavior.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any(f => f.PropertyName == "Name"));

        nextMock.Verify(n => n(It.IsAny<CancellationToken>()), Times.Never);
    }
}
