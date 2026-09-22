using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.UserModule.Models.Configurations;

/// <summary>
/// EF Core Fluent API configuration for the <see cref="Role"/> entity.
/// Maps to table [uvateafactory].[role].
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Table Name & Schema
        builder.ToTable("role", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(r => r.Name)
               .HasColumnName("name")
               .HasMaxLength(45);
    }
}
