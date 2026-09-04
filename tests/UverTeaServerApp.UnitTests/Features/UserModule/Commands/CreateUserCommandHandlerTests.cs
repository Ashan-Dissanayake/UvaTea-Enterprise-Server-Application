using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Commands.CreateUser;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Features.UserModule.Commands;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidUserCommand_ShouldHashPasswordAndPersistUser()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        var employee = new Employee
        {
            Number = "E100",
            Fullname = "Employee for User",
            GenderId = 1,
            DesignationId = 1,
            EmployeestatusId = 1,
            Createdat = DateTime.UtcNow
        };
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var mockUnitOfWork = new Mock<Shared.Data.IUnitOfWork>();
        mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken ct) => context.SaveChangesAsync(ct));

        var handler = new CreateUserCommandHandler(context,mockUnitOfWork.Object);
        var command = new CreateUserCommand(
            Username: "admin_user",
            Password: "SecurePassword123!",
            UserstatusId: 1,
            EmployeeId: employee.Id,
            RoleId: 1,
            Description: "System Administrator"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("admin_user");

        var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin_user");
        userInDb.Should().NotBeNull();
        userInDb!.Password.Should().NotBe("SecurePassword123!");
        userInDb.Password.Should().NotBeNullOrWhiteSpace();
    }
}
