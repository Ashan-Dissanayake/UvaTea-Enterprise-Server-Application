using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Configurations;

public class WeatherconditionConfiguration : IEntityTypeConfiguration<Weathercondition>
{
    public void Configure(EntityTypeBuilder<Weathercondition> builder)
    {
        // Table Name
        builder.ToTable("weathercondition", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        // Columns & Constraints
        builder.Property(w => w.Name)
               .HasColumnName("name")
               .HasMaxLength(45)
               .IsRequired();
    }
}