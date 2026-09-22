using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.FertilizerModule.Models.Configurations;

public class FertilizerStatusConfiguration : IEntityTypeConfiguration<Fertilizerstatus>
{
    public void Configure(EntityTypeBuilder<Fertilizerstatus> builder)
    {
        builder.ToTable("fertilizerstatus");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(s => s.Name)
            .IsUnique();
    }
}