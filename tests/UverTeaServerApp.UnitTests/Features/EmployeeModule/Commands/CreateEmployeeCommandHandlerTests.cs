using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.EmployeeModule.Commands.CreateEmployee;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Features.EmployeeModule.Commands;

public class CreateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistEmployeeAndReturnDto()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        var handler = new CreateEmployeeCommandHandler(context);
        var command = new CreateEmployeeCommand(
            Number: "E001",
            Fullname: "John Doe",
            Callingname: "John",
            Nic: "901234567V",
            Mobile: "0771234567",
            Land: "0112345678",
            Address: "123 Tea Estate Road, Badulla",
            Dobirth: new DateOnly(1990, 1, 1),
            Doassignment: new DateOnly(2020, 1, 1),
            GenderId: 1,
            DesignationId: 1,
            EmployeestatusId: 1,
            Description: "Field Manager"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Fullname.Should().Be("John Doe");
        result.Number.Should().Be("E001");

        var employeeInDb = await context.Employees.FirstOrDefaultAsync(e => e.Number == "E001");
        employeeInDb.Should().NotBeNull();
        employeeInDb!.Fullname.Should().Be("John Doe");
        employeeInDb.IsDeleted.Should().BeFalse();
    }
}
