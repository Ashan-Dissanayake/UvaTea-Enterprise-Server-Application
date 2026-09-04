using FluentAssertions;
using UverTeaServerApp.src.Feature.EmployeeModule.Commands.UpdateEmployee;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.UnitTests.Common;
using Moq;

namespace UverTeaServerApp.UnitTests.Features.EmployeeModule.Commands;

public class UpdateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ExistingEmployee_ShouldUpdateAndReturnDto()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        var employee = new Employee
        {
            Number = "E003",
            Fullname = "Original Name",
            Callingname = "Orig",
            Nic = "912345678V",
            GenderId = 1,
            DesignationId = 1,
            EmployeestatusId = 1,
            Createdat = DateTime.UtcNow
        };
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var mockUnitOfWork = new Mock<Shared.Data.IUnitOfWork>();

        var handler = new UpdateEmployeeCommandHandler(context,mockUnitOfWork.Object);
        var command = new UpdateEmployeeCommand(
            Id: employee.Id,
            Number: "E003",
            Fullname: "Updated Name",
            Callingname: "Updated",
            Email: "updated@example.com",
            Nic: "912345678V",
            Mobile: "0777777777",
            Land: null,
            Address: "Updated Address",
            Dobirth: null,
            Doassignment: null,
            GenderId: 1,
            DesignationId: 2,
            EmployeestatusId: 1,
            Description: "Updated desc"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Fullname.Should().Be("Updated Name");

        var updatedInDb = await context.Employees.FindAsync(employee.Id);
        updatedInDb!.Fullname.Should().Be("Updated Name");
        updatedInDb.DesignationId.Should().Be(2);
    }

    [Fact]
    public async Task Handle_NonExistingEmployee_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var mockUnitOfWork = new Mock<Shared.Data.IUnitOfWork>();

        var handler = new UpdateEmployeeCommandHandler(context,mockUnitOfWork.Object);
        var command = new UpdateEmployeeCommand(
            Id: 999,
            Number: "E999",
            Fullname: "Ghost",
            Callingname: null,
            Nic: "999999999V",
            Email: "ghost@example.com",
            Mobile: null,
            Land: null,
            Address: null,
            Dobirth: null,
            Doassignment: null,
            GenderId: 1,
            DesignationId: 1,
            EmployeestatusId: 1,
            Description: null
        );

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ResourceNotFoundException>()
            .WithMessage("*999*");
    }
}

