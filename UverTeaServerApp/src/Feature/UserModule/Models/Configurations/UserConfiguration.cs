using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.UserModule.Models.Configurations;

/// <summary>
/// EF Core Fluent API configuration for the <see cref="User"/> entity.
/// Mirrors the schema defined in the scaffolded UvateafactoryContext.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table Name & Schema
        builder.ToTable("user", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(u => u.Username)
               .HasColumnName("username")
               .HasMaxLength(45)
               .IsUnicode(false);

        builder.Property(u => u.Password)
               .HasColumnName("password")
               .HasMaxLength(255)
               .IsUnicode(false);

        builder.Property(u => u.Docreated)
               .HasColumnName("docreated");

        builder.Property(u => u.Tocreated)
               .HasColumnName("tocreated");

        builder.Property(u => u.UserstatusId)
               .HasColumnName("userstatus_id")
               .IsRequired();

        builder.Property(u => u.EmployeeId)
               .HasColumnName("employee_id")
               .IsRequired();

        builder.Property(u => u.RoleId)
               .HasColumnName("role_id")
               .IsRequired();

        builder.Property(u => u.Description)
               .HasColumnName("description")
               .HasColumnType("text");

        builder.Property(u => u.Createdat)
               .HasColumnName("createdat");

        builder.Property(u => u.Updatedat)
               .HasColumnName("updatedat");

        builder.Property(u => u.IsDeleted)
               .HasColumnName("isdeleted");

        // Indexes (matching scaffolded context)
        builder.HasIndex(u => u.EmployeeId, "fk_user_employee1_idx");
        builder.HasIndex(u => u.RoleId, "fk_user_role1_idx");
        builder.HasIndex(u => u.UserstatusId, "fk_user_userstatus1_idx");

        // Relationships / Foreign Keys
        builder.HasOne(u => u.Employee)
               .WithMany(e => e.Users)
               .HasForeignKey(u => u.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("fk_user_employee1");

        builder.HasOne(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("fk_user_role1");

        builder.HasOne(u => u.Userstatus)
               .WithMany(s => s.Users)
               .HasForeignKey(u => u.UserstatusId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("fk_user_userstatus1");
    }
}
