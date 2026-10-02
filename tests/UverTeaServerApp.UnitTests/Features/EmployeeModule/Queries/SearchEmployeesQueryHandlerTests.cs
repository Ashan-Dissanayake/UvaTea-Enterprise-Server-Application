using FluentAssertions;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.EmployeeModule.Queries.SeachEmployees;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Features.EmployeeModule.Queries;

public class SearchEmployeesQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithSearchTerm_ShouldReturnMatchingEmployees()
    {
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        context.Employees.AddRange(
            new Employee { Number = "E010", Fullname = "Alice Smith", Nic = "111111111V", Email = "alice@test.com", GenderId = 2, DesignationId = 1, EmployeestatusId = 1, Createdat = DateTime.UtcNow },
            new Employee { Number = "E020", Fullname = "Bob Johnson", Nic = "222222222V", Email = "bob@test.com", GenderId = 1, DesignationId = 2, EmployeestatusId = 1, Createdat = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var handler = new SearchEmployeesQueryHandler(context);
        var query = new SearchEmployeesQuery(
            new Dictionary<string, string?>(),
            new PaginationParams { SearchTerm = "Alice" }
        );

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().ContainSingle(e => e.Fullname == "Alice Smith");
    }

    [Fact]
    public async Task Handle_WithParamsDict_ShouldFilterByParameters()
    {
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        context.Employees.AddRange(
            new Employee { Number = "E010", Fullname = "Alice Smith", Nic = "111111111V", Email = "alice@test.com", GenderId = 2, DesignationId = 1, EmployeestatusId = 1, Createdat = DateTime.UtcNow },
            new Employee { Number = "E020", Fullname = "Bob Johnson", Nic = "222222222V", Email = "bob@test.com", GenderId = 1, DesignationId = 2, EmployeestatusId = 1, Createdat = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var handler = new SearchEmployeesQueryHandler(context);
        var query = new SearchEmployeesQuery(
            new Dictionary<string, string?> { { "number", "E020" } },
            new PaginationParams()
        );

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().ContainSingle(e => e.Number == "E020");
    }
}
