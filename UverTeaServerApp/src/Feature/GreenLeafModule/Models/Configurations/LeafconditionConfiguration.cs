using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class LeafconditionConfiguration : IEntityTypeConfiguration<Leafcondition>
{
    public void Configure(EntityTypeBuilder<Leafcondition> builder)
    {
        // Table Name
        builder.ToTable("leafcondition", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(s => s.Name)
               .HasColumnName("name")
               .HasMaxLength(45)
               .IsRequired();
    }
}