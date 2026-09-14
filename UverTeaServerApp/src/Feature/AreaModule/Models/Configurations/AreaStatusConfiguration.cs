using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;

public class AreaStatusConfiguration : IEntityTypeConfiguration<Areastatus>
{
    public void Configure(EntityTypeBuilder<Areastatus> builder)
    {
        builder.ToTable("areastatus", schema: "uvateafactory");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .HasMaxLength(45)
            .IsRequired();
    }
}