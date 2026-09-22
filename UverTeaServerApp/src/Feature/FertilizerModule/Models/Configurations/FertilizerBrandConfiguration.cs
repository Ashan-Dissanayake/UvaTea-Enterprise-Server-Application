using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.FertilizerModule.Models.Configurations;

public class FertilizerBrandConfiguration : IEntityTypeConfiguration<Fertilzerbrand>
{
    public void Configure(EntityTypeBuilder<Fertilzerbrand> builder)
    {
        builder.ToTable("fertilzerbrand");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}