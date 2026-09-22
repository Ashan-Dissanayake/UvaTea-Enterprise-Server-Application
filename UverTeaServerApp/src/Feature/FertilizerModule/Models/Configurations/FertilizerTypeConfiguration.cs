using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.FertilizerModule.Models.Configurations;

public class FertilizerTypeConfiguration : IEntityTypeConfiguration<Fertilizertype>
{
    public void Configure(EntityTypeBuilder<Fertilizertype> builder)
    {
        builder.ToTable("fertilizertype");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(t => t.Name)
            .IsUnique();
    }
}