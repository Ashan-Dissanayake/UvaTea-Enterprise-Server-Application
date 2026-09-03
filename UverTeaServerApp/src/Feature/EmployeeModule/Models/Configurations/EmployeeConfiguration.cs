using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Configurations;


public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
    {
        // Table Name
        builder.ToTable("employee",schema: "uvateafactory");

        // Primary Key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(e => e.Number)
               .HasColumnName("number")
               .HasMaxLength(4)
               .IsFixedLength();

        builder.Property(e => e.Fullname)
               .HasColumnName("fullname")
               .HasMaxLength(150);

        builder.Property(e => e.Callingname)
               .HasColumnName("callingname")
               .HasMaxLength(45);

        builder.Property(e => e.GenderId)
               .HasColumnName("gender_id")
               .IsRequired();

        builder.Property(e => e.Dobirth)
               .HasColumnName("dobirth")
               .HasColumnType("date");

        builder.Property(e => e.Nic)
               .HasColumnName("nic")
               .HasMaxLength(12)
               .IsFixedLength();

        builder.Property(e => e.Address)
               .HasColumnName("address")
               .HasColumnType("text");

        builder.Property(e => e.Mobile)
               .HasColumnName("mobile")
               .HasMaxLength(10)
               .IsFixedLength();

        builder.Property(e => e.Email)
               .HasColumnName("email")
               .HasMaxLength(50);

        builder.Property(e => e.Land)
               .HasColumnName("land")
               .HasMaxLength(10)
               .IsFixedLength();

        builder.Property(e => e.Doassignment)
               .HasColumnName("doassignment")
               .HasColumnType("date");

        builder.Property(e => e.DesignationId)
               .HasColumnName("designation_id")
               .IsRequired();

        builder.Property(e => e.EmployeestatusId)
               .HasColumnName("employeestatus_id")
               .IsRequired();

        builder.Property(e => e.Description)
               .HasColumnName("description")
               .HasColumnType("text");

        builder.Property(e => e.IsDeleted)
               .HasColumnName("isdeleted");

        // Relationships Setup (Foreign Keys)
        builder.HasOne(e => e.Gender)
               .WithMany(g => g.Employees)
               .HasForeignKey(e => e.GenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Designation)
               .WithMany(d => d.Employees)
               .HasForeignKey(e => e.DesignationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Employeestatus)
               .WithMany(es => es.Employees)
               .HasForeignKey(e => e.EmployeestatusId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}