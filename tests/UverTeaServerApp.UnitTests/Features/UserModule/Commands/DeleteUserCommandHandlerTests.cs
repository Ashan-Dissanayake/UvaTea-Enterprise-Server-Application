using FluentAssertions;
using MediatR;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Commands.DeleteUser;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Features.UserModule.Commands;

public class DeleteUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ExistingUser_ShouldRemoveUser()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        var employee = new Employee
        {
            Number = "E101",
            Fullname = "Emp 101",
            GenderId = 1,
            DesignationId = 1,
            EmployeestatusId = 1,
            Createdat = DateTime.UtcNow
        };
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var user = new User
        {
            Username = "user_to_delete",
            Password = "hashedpassword",
            EmployeeId = employee.Id,
            RoleId = 2,
            UserstatusId = 1,
            Createdat = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new DeleteUserCommandHandler(context);
        var command = new DeleteUserCommand(user.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        var deletedUser = await context.Users.FindAsync(user.Id);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NonExistingUser_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var handler = new DeleteUserCommandHandler(context);
        var command = new DeleteUserCommand(999);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage("*999*");
    }
}

