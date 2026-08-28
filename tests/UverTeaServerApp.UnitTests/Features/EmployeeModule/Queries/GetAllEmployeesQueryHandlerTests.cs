using FluentAssertions;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.EmployeeModule.Queries.GetAllEmployees;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Features.EmployeeModule.Queries;

public class GetAllEmployeesQueryHandlerTests
{
    [Fact]
    public async Task Handle_DefaultPagination_ShouldReturnPaginatedList()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        for (int i = 1; i <= 15; i++)
        {
            context.Employees.Add(new Employee
            {
                Number = $"E{i:D3}",
                Fullname = $"Employee {i}",
                GenderId = 1,
                DesignationId = 1,
                EmployeestatusId = 1,
                Createdat = DateTime.UtcNow
            });
        }
        await context.SaveChangesAsync();

        var handler = new GetAllEmployeesQueryHandler(context);
        var query = new GetAllEmployeesQuery(new PaginationParams
        {
            PageNumber = 1,
            PageSize = 10
        });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(15);
        result.TotalPages.Should().Be(2);
        result.Items.Count.Should().Be(10);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WithSearchTerm_ShouldReturnFilteredResults()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        context.Employees.AddRange(
            new Employee { Number = "E010", Fullname = "Alice Smith", Nic = "111111111V", GenderId = 2, DesignationId = 1, EmployeestatusId = 1, Createdat = DateTime.UtcNow },
            new Employee { Number = "E020", Fullname = "Bob Johnson", Nic = "222222222V", GenderId = 1, DesignationId = 2, EmployeestatusId = 1, Createdat = DateTime.UtcNow },
            new Employee { Number = "E030", Fullname = "Alice Cooper", Nic = "333333333V", GenderId = 2, DesignationId = 1, EmployeestatusId = 1, Createdat = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var handler = new GetAllEmployeesQueryHandler(context);
        var query = new GetAllEmployeesQuery(new PaginationParams
        {
            SearchTerm = "Alice",
            PageNumber = 1,
            PageSize = 10
        });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(e => e.Fullname != null && e.Fullname.Contains("Alice"));
    }
}
