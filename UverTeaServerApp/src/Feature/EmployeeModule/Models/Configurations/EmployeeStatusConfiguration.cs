using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Configurations;
public class EmployeeStatusConfiguration : IEntityTypeConfiguration<Employeestatus>
{
    public void Configure(EntityTypeBuilder<Employeestatus> builder)
    {
        // Table Name
        builder.ToTable("employeestatus",schema: "uvateafactory");

        // Primary Key
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(g => g.Name)
               .HasColumnName("name")
               .HasMaxLength(45)
               .IsRequired();
    }
}