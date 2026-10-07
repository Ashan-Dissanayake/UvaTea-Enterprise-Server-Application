using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class ContainertypeConfiguration : IEntityTypeConfiguration<Containertype>
{
    public void Configure(EntityTypeBuilder<Containertype> builder)
    {
        // Table Name
        builder.ToTable("containertype", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(c => c.Name)
               .HasColumnName("name")
               .HasMaxLength(45)
               .IsRequired();
    }
}