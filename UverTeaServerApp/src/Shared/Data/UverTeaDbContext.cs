using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Entities;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.Data;

public class UvaTeaDbContext : DbContext
{
    public UvaTeaDbContext(DbContextOptions<UvaTeaDbContext> options) : base(options) 
    { 
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Employeestatus> EmployeeStatuses { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Designation> Designations { get; set; }
    public DbSet<Userstatus> Userstatuses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Employee>().ToTable("employee", "uvateafactory");
        modelBuilder.Entity<Employeestatus>().ToTable("employeestatus", "uvateafactory");
        modelBuilder.Entity<Gender>().ToTable("gender", "uvateafactory");
        modelBuilder.Entity<Designation>().ToTable("designation", "uvateafactory");
        modelBuilder.Entity<Userstatus>().ToTable("userstatus", "uvateafactory");
        modelBuilder.Entity<Role>().ToTable("role", "uvateafactory");
        modelBuilder.Entity<User>().ToTable("user", "uvateafactory");

        // Explicit isdeleted column mapping and Global Query Filters
        modelBuilder.Entity<Employee>().Property(e => e.IsDeleted).HasColumnName("isdeleted");
        modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<User>().Property(u => u.IsDeleted).HasColumnName("isdeleted");
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);

        // Dynamic Global Query Filters and column mapping for all ISoftDeletable entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(ISoftDeletable.IsDeleted))
                    .HasColumnName("isdeleted");

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethodInfo = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)
                    ?.MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo!, parameter, Expression.Constant(nameof(ISoftDeletable.IsDeleted)));
                var compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UvaTeaDbContext).Assembly);
    }
}