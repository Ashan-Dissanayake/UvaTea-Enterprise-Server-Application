using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Entities;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.Shared.Data;

public class UvaTeaDbContext : DbContext
{
    public UvaTeaDbContext(DbContextOptions<UvaTeaDbContext> options)
        : base(options)
    {
    }

    // =========================================================
    // Employee
    // =========================================================

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Employeestatus> EmployeeStatuses { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Designation> Designations { get; set; }

    // =========================================================
    // User
    // =========================================================

    public DbSet<Userstatus> Userstatuses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    // =========================================================
    // Area
    // =========================================================

    public DbSet<Areastatus> Areastatuses { get; set; }
    public DbSet<Areacategory> Areacategories { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Growthstage> Growthstages { get; set; }
    public DbSet<Plantingconfiguration> Plantingconfigurations { get; set; }
    public DbSet<Areagrowthstagehistory> Areagrowthstagehistories { get; set; }
    public DbSet<Areastatushistory> Areastatushistories { get; set; }

    // =========================================================
    // Fertilizer
    // =========================================================

    public DbSet<Fertilizer> Fertilizers { get; set; }

    public DbSet<Fertilizerstatus> FertilizerStatuses { get; set; }

    public DbSet<Fertilizertype> FertilizerTypes { get; set; }

    public DbSet<Fertilzerbrand> FertilizerBrands { get; set; }

    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================================================
        // Global Soft Delete Configuration
        // =========================================================

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable)
                .IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(ISoftDeletable.IsDeleted))
                .HasColumnName("isdeleted");

            var parameter = Expression.Parameter(
                entityType.ClrType,
                "e");

            var propertyMethodInfo = typeof(EF)
                .GetMethod(
                    nameof(EF.Property),
                    BindingFlags.Static | BindingFlags.Public)
                ?.MakeGenericMethod(typeof(bool));

            var isDeletedProperty = Expression.Call(
                propertyMethodInfo!,
                parameter,
                Expression.Constant(
                    nameof(ISoftDeletable.IsDeleted)));

            var compareExpression = Expression.Equal(
                isDeletedProperty,
                Expression.Constant(false));

            var lambda = Expression.Lambda(
                compareExpression,
                parameter);

            modelBuilder.Entity(entityType.ClrType)
                .HasQueryFilter(lambda);
        }

        // =========================================================
        // Entity Configurations
        // =========================================================

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UvaTeaDbContext).Assembly);
    }
}