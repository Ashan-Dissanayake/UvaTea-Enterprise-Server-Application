using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.UserModule.Models.Configurations;

/// <summary>
/// EF Core Fluent API configuration for the <see cref="Userstatus"/> entity.
/// Maps to table [uvateafactory].[userstatus].
/// </summary>
public class UserstatusConfiguration : IEntityTypeConfiguration<Userstatus>
{
    public void Configure(EntityTypeBuilder<Userstatus> builder)
    {
        // Table Name & Schema
        builder.ToTable("userstatus", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(s => s.Name)
               .HasColumnName("name")
               .HasMaxLength(45);
    }
}
