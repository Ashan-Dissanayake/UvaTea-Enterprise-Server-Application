using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;

public class GrowthStageConfiguration : IEntityTypeConfiguration<Growthstage>
{
    public void Configure(EntityTypeBuilder<Growthstage> builder)
    {
        builder.ToTable("growthstage", schema: "uvateafactory");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(g => g.Displayorder)
            .HasColumnName("displayorder")
            .IsRequired();

        builder.Property(g => g.Isactive)
            .HasColumnName("isactive")
            .IsRequired();
    }
}