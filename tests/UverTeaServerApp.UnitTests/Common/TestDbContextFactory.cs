using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.UnitTests.Common;

public static class TestDbContextFactory
{
    public static UvaTeaDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(databaseName: string.IsNullOrWhiteSpace(dbName) ? Guid.NewGuid().ToString() : dbName)
            .Options;

        var context = new UvaTeaDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static IUnitOfWork CreateUnitOfWork(UvaTeaDbContext context)
    {
        return new UnitOfWork(context);
    }
}

