using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.src.Feature.EmployeeModule.Commands.DeleteEmployee;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Features.EmployeeModule.Commands;

public class DeleteEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ExistingEmployee_ShouldRemoveEmployee()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        var employee = new Employee
        {
            Number = "E002",
            Fullname = "Jane Doe",
            GenderId = 2,
            DesignationId = 2,
            EmployeestatusId = 1,
            Createdat = DateTime.UtcNow
        };
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var handler = new DeleteEmployeeCommandHandler(context);
        var command = new DeleteEmployeeCommand(employee.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        
        var deletedEmployee = await context.Employees.FindAsync(employee.Id);
        deletedEmployee.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NonExistingEmployee_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var handler = new DeleteEmployeeCommandHandler(context);
        var command = new DeleteEmployeeCommand(999);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage("*999*");
    }
}

