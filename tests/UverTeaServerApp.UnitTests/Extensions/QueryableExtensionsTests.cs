using FluentAssertions;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.Shared.Extensions;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.UnitTests.Common;

namespace UverTeaServerApp.UnitTests.Extensions;

public class QueryableExtensionsTests
{
    [Fact]
    public async Task ToPagedResultAsync_ShouldCalculatePaginationMetadataCorrectly()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        MockDataGenerator.SeedMasterData(context);

        for (int i = 1; i <= 25; i++)
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

        var query = context.Employees.AsQueryable();

        // Act
        var pagedResult = await query.ToPagedResultAsync(pageNumber: 2, pageSize: 10);

        // Assert
        pagedResult.Should().NotBeNull();
        pagedResult.TotalCount.Should().Be(25);
        pagedResult.TotalPages.Should().Be(3);
        pagedResult.PageNumber.Should().Be(2);
        pagedResult.PageSize.Should().Be(10);
        pagedResult.Items.Count.Should().Be(10);
        pagedResult.HasPreviousPage.Should().BeTrue();
        pagedResult.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void ApplySort_AscendingAndDescending_ShouldSortCorrectly()
    {
        // Arrange
        var list = new List<Employee>
        {
            new() { Id = 1, Fullname = "Charlie", Number = "E003" },
            new() { Id = 2, Fullname = "Alice", Number = "E001" },
            new() { Id = 3, Fullname = "Bob", Number = "E002" }
        }.AsQueryable();

        // Act - Ascending
        var sortedAsc = list.ApplySort("Fullname", "asc").ToList();
        // Act - Descending
        var sortedDesc = list.ApplySort("Fullname", "desc").ToList();

        // Assert
        sortedAsc[0].Fullname.Should().Be("Alice");
        sortedAsc[1].Fullname.Should().Be("Bob");
        sortedAsc[2].Fullname.Should().Be("Charlie");

        sortedDesc[0].Fullname.Should().Be("Charlie");
        sortedDesc[1].Fullname.Should().Be("Bob");
        sortedDesc[2].Fullname.Should().Be("Alice");
    }
}
