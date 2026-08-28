using UverTeaServerApp.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.UnitTests.Common;

public static class MockDataGenerator
{
    public static void SeedMasterData(UvaTeaDbContext context)
    {
        if (!context.Genders.Any())
        {
            context.Genders.AddRange(
                new Gender { Id = 1, Name = "Male" },
                new Gender { Id = 2, Name = "Female" }
            );
        }

        if (!context.Designations.Any())
        {
            context.Designations.AddRange(
                new Designation { Id = 1, Name = "Manager" },
                new Designation { Id = 2, Name = "Supervisor" },
                new Designation { Id = 3, Name = "Worker" }
            );
        }

        if (!context.EmployeeStatuses.Any())
        {
            context.EmployeeStatuses.AddRange(
                new Employeestatus { Id = 1, Name = "Active" },
                new Employeestatus { Id = 2, Name = "Resigned" },
                new Employeestatus { Id = 3, Name = "Suspended" }
            );
        }

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Manager" },
                new Role { Id = 3, Name = "Employee" }
            );
        }

        if (!context.Userstatuses.Any())
        {
            context.Userstatuses.AddRange(
                new Userstatus { Id = 1, Name = "Active" },
                new Userstatus { Id = 2, Name = "Inactive" }
            );
        }

        context.SaveChanges();
    }
}
