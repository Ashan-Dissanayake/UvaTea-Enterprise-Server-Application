using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;
public class AreaCategoryConfiguration : IEntityTypeConfiguration<Areacategory>
{
    public void Configure(EntityTypeBuilder<Areacategory> builder)
    {
        // Table Name
        builder.ToTable("areacategory",schema: "uvateafactory");

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