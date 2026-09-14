using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;

public class PlantingConfigurationConfiguration
    : IEntityTypeConfiguration<Plantingconfiguration>
{
    public void Configure(
        EntityTypeBuilder<Plantingconfiguration> builder)
    {
        builder.ToTable(
            "plantingconfiguration",
            schema: "uvateafactory");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Rowspacingfeet)
            .HasColumnName("rowspacingfeet")
            .HasColumnType("decimal(6,2)")
            .IsRequired();

        builder.Property(p => p.Plantspacingfeet)
            .HasColumnName("plantspacingfeet")
            .HasColumnType("decimal(6,2)")
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(p => p.Isactive)
            .HasColumnName("isactive")
            .IsRequired();

        builder.Property(p => p.Createdat)
            .HasColumnName("createdat")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(p => p.Updatedat)
            .HasColumnName("updatedat")
            .HasColumnType("datetime2");

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}