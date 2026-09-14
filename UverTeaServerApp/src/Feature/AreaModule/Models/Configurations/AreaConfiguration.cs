using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        // Table
        builder.ToTable("area", schema: "uvateafactory");

        // Primary Key
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Columns
        builder.Property(a => a.Code)
            .HasColumnName("code")
            .HasMaxLength(5)
            .IsUnicode(false)
            .IsFixedLength();

        builder.Property(a => a.Acres)
            .HasColumnName("acres")
            .HasColumnType("decimal(7,2)");

        builder.Property(a => a.DoAttached)
            .HasColumnName("doattached")
            .HasColumnType("date");

        builder.Property(a => a.PlantCount)
            .HasColumnName("plantcount");

        builder.Property(a => a.DoProofing)
            .HasColumnName("doproofing")
            .HasColumnType("date");

        builder.Property(a => a.SupervisorId)
            .HasColumnName("supervisor_id");

        builder.Property(a => a.AreaStatusId)
            .HasColumnName("areastatus_id")
            .IsRequired();

        builder.Property(a => a.AreaCategoryId)
            .HasColumnName("areacategory_id")
            .IsRequired();

        builder.Property(a => a.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(a => a.GrowthStageId)
            .HasColumnName("growthstage_id");

        builder.Property(a => a.PlantingConfigurationId)
            .HasColumnName("plantingconfiguration_id");

        // Relationships

        builder.HasOne(a => a.Supervisor)
            .WithMany(e => e.Areas)
            .HasForeignKey(a => a.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.AreaStatus)
            .WithMany(s => s.Areas)
            .HasForeignKey(a => a.AreaStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.AreaCategory)
            .WithMany(c => c.Areas)
            .HasForeignKey(a => a.AreaCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.User)
            .WithMany(u => u.Areas)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.GrowthStage)
            .WithMany(g => g.Areas)
            .HasForeignKey(a => a.GrowthStageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.PlantingConfiguration)
            .WithMany(p => p.Areas)
            .HasForeignKey(a => a.PlantingConfigurationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}